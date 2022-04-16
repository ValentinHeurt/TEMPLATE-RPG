using UnityEngine;
[CreateAssetMenu(fileName = "New void Event", menuName = "Game Events/void Event")]
public class VoidEvent : BaseGameEvent<Void>
{
    public void Raise() => Raise(new Void());
}
