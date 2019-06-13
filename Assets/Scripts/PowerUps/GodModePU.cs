//// Clase derivada/Hija de PowerUp. Un poder que nos vuelve invulnerables
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModePU : PowerUp
{
    private GameObject EffectGodMode; // Referencia a la animacion/efecto

    public void MakeYourMagic(bool godMode) {
        // Metodo sobrecargado, aca lo que se controla es si el metodo va a ser parcial (desde PowerUp) o total (desde truco de consola)
        if (godMode) {
            // Si le digo que quiero el godMode llama al metodo de invulnerabilidad y nunca invocara la reversion
            this.SetInVulnerable();
        }
        else {
            // Si le digo que es solo parcial (desde powerUp) llama al metodo original y se invocara la reversion
            this.MakeYourMagic();
        }
    }

    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp
        this.SetInVulnerable(); // Llama al metodo para hacerse invulerable
        Invoke("RevertYourMagic", this.GetCoolTime());  // Revierto el powerUp en CoolTime segundos
    }


    private void SetInVulnerable() {
        // Le pido al pool que active la animacion del powerUp
        EffectGodMode = this.GetPool().Spawn("ShieldedAnimation", this.GetAsimov().transform.position, this.GetAsimov().transform.rotation);
        // la animacion es hija de la nave (en la jerarquia) de manera tal que se mueva y rote con ella
        EffectGodMode.transform.parent = this.GetAsimov().transform;

        this.GetAsimov().SetIsVulnerable(false); // Le digo a la nave que no es vulnerable
    }

    private void RevertYourMagic() {
        // Metodo que revierte el PowerUp
        this.GetAsimov().SetIsVulnerable(true); // Le digo a la nave que vuelve a ser vulnerable
    }
}
