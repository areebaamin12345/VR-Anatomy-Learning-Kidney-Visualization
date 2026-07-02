using UnityEngine;
using System.Collections.Generic;

public class BloodFlow : MonoBehaviour
{
    public Transform[] waypoints; // This will hold your Target_01, Target_02, etc.
    public float speed = 5.0f;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Start() { ps = GetComponent<ParticleSystem>(); }

    void LateUpdate()
    {
        if (particles == null || particles.Length < ps.main.maxParticles)
            particles = new ParticleSystem.Particle[ps.main.maxParticles];

        int numParticlesAlive = ps.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            // This pulls each particle toward the first waypoint in the list
            // For a complex branching path, we usually use 'Velocity over Lifetime'
            // but for a single path, this MoveTowards works best.
            particles[i].position = Vector3.MoveTowards(particles[i].position, waypoints[0].position, speed * Time.deltaTime);
        }

        ps.SetParticles(particles, numParticlesAlive);
    }
}