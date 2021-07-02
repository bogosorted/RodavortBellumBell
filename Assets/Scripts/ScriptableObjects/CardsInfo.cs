using UnityEngine;

[CreateAssetMenu(fileName = "New Card",menuName = "Cards/Normal")]
public class CardsInfo : ScriptableObject
{
    public uint cardId;

    [Header("Reading")]
    public Sprite design;
    
    [Header("Attributes")]
    public int gold;
    public int life;
    public int power;

    [Header("Dubbing")]
    public AudioClip[] SoundAtEntrance;
    public AudioClip[] SoundInDeath;

}