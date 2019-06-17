using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpImageControl : MonoBehaviour
{
    private Image Img;

    private Color OffImg = new Color(255f, 255f, 255f, 0f);
    private Color OnImg = new Color(255f, 255f, 255f, 255f);

    [SerializeField] private List<Sprite> PowerUpsImg;

    private Dictionary<string, Sprite> PowerUpImages = new Dictionary<string, Sprite>();


    private void Start() {
        this.PowerUpImages.Add("PowerUpSpeedBoost(Clone)", PowerUpsImg[0]);
        this.PowerUpImages.Add("PowerUpBigShield(Clone)", PowerUpsImg[1]);
        this.PowerUpImages.Add("PowerUpRestoreHealth(Clone)", PowerUpsImg[2]);
        this.PowerUpImages.Add("PowerUpGodMode(Clone)", PowerUpsImg[3]);
        this.PowerUpImages.Add("PowerUpSpiralShieldShoot(Clone)", PowerUpsImg[4]);
        this.PowerUpImages.Add("PowerUpDrones(Clone)", PowerUpsImg[5]);
        this.PowerUpImages.Add("PowerUpFrontalShieldShoot(Clone)", PowerUpsImg[6]);
        this.PowerUpImages.Add("PowerUpAirMines(Clone)", PowerUpsImg[7]);
        this.PowerUpImages.Add("PowerUpBulletTime(Clone)", PowerUpsImg[8]);
        this.PowerUpImages.Add("PowerUpClone(Clone)", PowerUpsImg[9]);

        this.Img = GetComponent<Image>();

        this.Img.sprite = PowerUpsImg[0];
        this.NoImage();
    }

    public void NoImage() {
        this.Img.color = OffImg;
    }

    public void SetImage(PowerUp powerUp) {
        this.Img.sprite = this.PowerUpImages[powerUp.name];
        this.Img.color = OnImg;
    }

}
