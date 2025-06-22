using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int swordPower = 10;
    public int SwordPower => swordPower;

    [SerializeField] private ParticleSystem swordParticle;
    public ParticleSystem SwordParticle => swordParticle;
}
