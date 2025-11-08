using UnityEngine;

public interface IHoldable
{
    public void Equip();
    public void UnEquip();
}

public enum HoldItemType
{
    Gun,
    BasketBall
}


