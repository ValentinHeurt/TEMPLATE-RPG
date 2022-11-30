using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MetierWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentMetierName;
    [SerializeField] private List<Metier> metiers;
    public Transform metiersContainer;
    public Transform metierUsagesContainer;
    public MetierUIHandler metierUIPrefab;
    public MetierUsageUIHandler metierUsageUIPrefab;
    public Metier currentMetier;
    private void OnEnable()
    {
        EventManager.Instance.AddListener<OnMetierSelected>(SetDisplayedMetier);
        foreach (Transform metier in metiersContainer)
        {
            GameObject.Destroy(metier.gameObject);
        }
        foreach (Transform metierUsage in metierUsagesContainer)
        {
            GameObject.Destroy(metierUsage.gameObject);
        }

        metiers.ForEach(metier =>
        {
            MetierUIHandler tempMetier = Instantiate(metierUIPrefab, metiersContainer);
            tempMetier.FillInformations(metier);
        });


        if (currentMetier != null)
            EventManager.Instance.QueueEvent(new OnMetierSelected(currentMetier));
        else
            EventManager.Instance.QueueEvent(new OnMetierSelected(metiers[0]));
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<OnMetierSelected>(SetDisplayedMetier);
    }


    public void SetDisplayedMetier(OnMetierSelected eventInfo)
    {
        currentMetier = eventInfo.metier;
        currentMetierName.text = currentMetier.Name;
        foreach (Transform usage in metierUsagesContainer)
        {
            GameObject.Destroy(usage.gameObject);
        }
        currentMetier.harvestableData.ForEach(data =>
        {
            MetierUsageUIHandler tempUsage = Instantiate(metierUsageUIPrefab, metierUsagesContainer);
            tempUsage.FillInformations(data.item.name, data.minLvlToHarvest.ToString(), data.item.icon);
        });
    }

}
