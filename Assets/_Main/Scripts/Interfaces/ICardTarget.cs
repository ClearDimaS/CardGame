using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardTarget 
{
    public bool TryInteract(Card card);
}
