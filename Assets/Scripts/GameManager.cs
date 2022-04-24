using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public Player player;

    public ParticleSystem explosion;

    public int lives = 3;

    public float respawnTime = 3.0f;

    public float InvulnerabilityTime = 3.0f;

    public int score = 0;

    public static GameManager gm;

    public static GameManager Instance() {
        if (!gm) {
            gm = FindObjectOfType(typeof(GameManager)) as GameManager;
        }

        return gm;
    }

    public void PlayerDied() {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        if (--this.lives <= 0) {
            GameOver();
            return;
        }

        Invoke(nameof(Respawn), this.respawnTime);
    }

    private void Respawn() {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collision");
        this.player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollision), this.InvulnerabilityTime);
    }

    private void TurnOnCollision() {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver() {
        this.lives = 3;
        this.score = 0;
        Respawn();
    }

    public void AsteroidDestroyed(Asteroid asteroid) {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        this.score += (int) ((1 / asteroid.size) * 20.0f);
    }
}