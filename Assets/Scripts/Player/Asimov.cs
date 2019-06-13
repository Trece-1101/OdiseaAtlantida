//// Clase Derivada/Hija de "SHIP" que describe a al objeto de la Nave que controla el Player
// GodMode up, up, down, down, left, right, left, right, B, A.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asimov : Ship
{
    #region "Atributos"
    private Vector2 OriginalVelocity; // Velocidad Original de la nave
    private bool CanDash; // Controla si la nave puede hacer el movimiento "Dash"
    private float DashCoolTime; // Tiempo de recarga para volver a dashear
    private float InitialDashCoolTime = 2f; // Tiempo inicial de recarga
    private float DashSpeed = 3f; // Velocidad del Dash
    private float DashDistance = 4f; // Distancia del dash
    private float DashStep = 0.5f; // Cantidad de pasos para llegar a la distancia

    private bool CanRotate = true; // Atributo que controla si la nave puede rotar
    private bool IsCloned = false; // Atributo que chequea si la nave esta clonada o no

    private string Mode = "Attack"; // String de chequeo del modo actual de la nave
    private float TransitionDelay = 3f; // Tiempo de espera para lograr la transicion de un modo a otro
    private bool InTransition = false; // Atributo que chequea si se esta en proceso de transicion
    private float TransitionValueModifier = 1.5f; // Valor que modifica otros atributos dependiendo el modo de la nave

    private bool HasPowerUp; // Atributo que controla si la nave tiene un PowerUp asignado (consumido) sin usar
    private PowerUp PowerUpType; // Atributo de tipo PowerUp
    #endregion      

    #region "Referencias en Cache"     
    private Shield MyShield; // Atributo del tipo Shield, es el escudo de la nave
    private PolygonCollider2D AtackCollider; // Referencia al colisionador en modo ataque
    private PolygonCollider2D DefenseCollider; // Referencia al colisionador en modo defensa
    private Animator MyAnimator; // Referencia al animator de la nave
    #endregion

    #region "Setters/Getters"
    public float GetDashCoolTime() {
        return this.DashCoolTime;
    }
    public void SetDashCoolTime(float value) {
        this.DashCoolTime = value;
    }

    public float GetInitialDashCoolTime() {
        return this.InitialDashCoolTime;
    }
    public void SetInitialDashCoolTime(float value) {
        this.InitialDashCoolTime = value;
    }

    public float GetDashSpeed() {
        return this.DashSpeed;
    }
    public void SetDashSpeed(float value) {
        this.DashSpeed = value;
    }

    public float GetDashDistance() {
        return this.DashDistance;
    }
    public void SetDashDistance(float value) {
        this.DashDistance = value;
    }

    public float GetDashStep() {
        return this.DashStep;
    }
    public void SetDashStep(float value) {
        this.DashStep = value;
    }

    public bool GetCanDash() {
        return this.CanDash;
    }
    public void SetCanDash(bool value) {
        this.CanDash = value;
    }

    public bool GetCanRotate() {
        return this.CanRotate;
    }
    public void SetGetCanRotate(bool value) {
        this.CanRotate = value;
    }


    public Shield GetMyShield() {
        return this.MyShield;
    }
    public void SetMyShield(Shield value) {
        this.MyShield = value;
    }

    public PolygonCollider2D GetAtackCollider() {
        return this.AtackCollider;
    }
    public void SetAtackColiider(PolygonCollider2D value) {
        this.AtackCollider = value;
    }

    public PolygonCollider2D GetDefenseCollider() {
        return this.DefenseCollider;
    }
    public void SetDefenseCollider(PolygonCollider2D value) {
        this.DefenseCollider = value;
    }

    public bool GetHasPowerUp() {
        return this.HasPowerUp;
    }
    public void SetHasPowerUp(bool value) {
        this.HasPowerUp = value;
    }

    public bool GetIsCloned() {
        return this.IsCloned;
    }
    public void SetIsCloned(bool value) {
        this.IsCloned = value;
    }

    public Vector2 GetOriginalVelocity() {
        return this.OriginalVelocity;
    }
    public void SetOriginalVelocity(Vector2 value) {
        this.OriginalVelocity = value;
    }

    public PowerUp GetPowerUpType() {
        return this.PowerUpType;
    }
    public void SetPowerUpType(PowerUp value) {
        this.PowerUpType = value;
    }
    #endregion

    #region "Metodos"

    public override void Awake() {
        base.Awake();

        // Enlazamos los componentes en cache con sus respectivas referencias
        this.MyShield = FindObjectOfType<Shield>();
        this.MyAnimator = GetComponent<Animator>();

        PolygonCollider2D[] colliders = GetComponents<PolygonCollider2D>();
        this.AtackCollider = colliders[0];
        this.DefenseCollider = colliders[1];

        this.OriginalVelocity = this.GetVelocity();
        this.DashCoolTime = this.InitialDashCoolTime;
        this.SetOriginalHitPoints(this.GetHitPoints());



        this.SetTimeBetweenBulletShoots(0.15f);
        this.SetTimeBetweenMissileShoots(1.5f);
        this.RemainTimeForShootBullet = this.GetTimeBetweenBulletShoots();
        this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();

        this.CanShootMissile = true;
        this.CanShoot = true;
    }
   
    private void Update() {
        this.CheckRotation(); // Metodo que chequea la rotacion y rota al player
        this.Move(); // Metodo que para mover al player en sentido horizontal y vertical
        this.Shoot(); // Metodo que controla los disparos del Player
        this.Dash(); // Metodo que controla la accion de Dash
        this.UsePowerUp(); // Metodo para ejecutar el powerUp consumido
        this.MoveShield(); // Metodo para manejar el escudo
        this.RestartShield(); // Metodo que reinicia el escudo
        this.CheckMode(); // Metodo que controla el modo actual o el cambio de modo de la nave (ofensivo o defensivo)
    }

    public override void CheckRotation() {
        // vector_direccion_ataque = vector_posicion_mouse - vector_centro_camara
        // 90 grados para compensar que el sprite tiene su 0° hacia el Norte y la camara tiene sus 0° hacia el Este
        // el sprite del proyectil ya apunta hacia el Este asi que no tiene compensacion
        var rotations = this.Rotate(Input.mousePosition - this.GetMyMainCamera().WorldToScreenPoint(this.transform.position), 90, 0);

        // Si la nave no puede Rotar queda mirando hacia el norte (sprite original)
        if (!CanRotate) {
            rotations = this.Rotate(new Vector3(0f, 0f, 0f), 0, 0);
        }

        // Asignamos las rotaciones correspondientes
        this.SetMyRotation(rotations["rotation_ship"]);
        this.SetMyBulletRotation(rotations["rotation_bullet"]);
        this.transform.rotation = rotations["transform_rotation"];
    }

    public override void Move() {
        // Metodo para controlar el input de usuario en movimientos horizontales y verticales

        var movementX = Input.GetAxis("Horizontal"); // -1 a 0 => Izquierda -- 0 => Sin Movimiento (No Input) -- 0 a 1 => Derecha
        var movementY = Input.GetAxis("Vertical"); // -1 a 0 => Abajo -- 0 => Sin Movimiento (No Input) -- 0 a 1 => Arriba               

        // d = v*t
        // si el input = 0 --> deltaMov = 0
        var deltaX = movementX * GetVelocity().x * Time.deltaTime; 
        var deltaY = movementY * GetVelocity().y * Time.deltaTime;

        // Descomentar para ver valores de movimiento
        //Debug.Log($"MovX {movementX} m/s -- MovY {movementY} m/s");
        //Debug.Log($"DeltaX {deltaX} m -- DeltaY {deltaY} m");

        // proxima pos = posicion actual + deltaMov -- bloqueo mi proxima posicion para que no pueda avanzar mas alla de los margenes
        // pos(n) = pos(n-1) + v*t -- v*t = deltaMov
        var nextPosX = Mathf.Clamp(transform.position.x + deltaX * this.GetGameProg().GetScale().x, this.GetGameProg().GetLeftBorder(), this.GetGameProg().GetRightBorder());
        var nextPosY = Mathf.Clamp(transform.position.y + deltaY * this.GetGameProg().GetScale().y, this.GetGameProg().GetUpBorder(), this.GetGameProg().GetDownBorder());

        Vector2 nextPosition = new Vector2(nextPosX, nextPosY);

        // mi vector posicion ahora vale la posicion 'x' e 'y' calculadas        
        this.transform.position = nextPosition;
    }

    public override void Shoot() {
        // Metodo que controla el disparo de la nave
        // Si el enemigo puede disparar y el contador de tiempo llego a 0 => dispara
        // Tambien se controla si el enemigo tiene puntos de disparo. Si no los tuviera en el metodo "ShootBullet" no pasaria nada
        // Porque iteraria en un for de 0 a 0. Pero se controla aca para evitar tener que entrar al IF y ser mas eficiente
        if (this.RemainTimeForShootBullet <= 0 && this.CanShoot) {
            // si ya paso el tiempo para poder disparar y puede hacerlo y se presiona el boton "Fire 1"
            if (Input.GetButton("Fire1")) {
                this.ShootBullet(); // Metodo en la clase padre que dispara la bala
                this.ShootMicroBullet(); // Metodo en la clase padre que dispara la bala secundaria
                this.PlayShootSFX(this.GetShootBulletSFX(), this.GetMyMainCamera().transform.position, 0.2f); // Metodo para realizar el sonido del disparo

                this.RemainTimeForShootBullet = this.GetTimeBetweenBulletShoots(); // Regresamos el contador a su valor original
            }
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime; // Si aun no es 0 descontamos un deltaTime
        }

        if (this.RemainTimeForShootMissile <= 0 && this.CanShootMissile) {
            // este metodo funciona identicamente que el de disparar balas pero con el boton de "Fire2"
            if (Input.GetButtonDown("Fire2")) {
                ShootMissile();
                PlayShootSFX(this.GetShootMissileSFX(), this.GetMyMainCamera().transform.position, 0.2f);

                this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
            }
        }
        else {
            this.RemainTimeForShootMissile -= Time.deltaTime;
        }
    }

    private void Dash() {
        // Metodo que controla el movimiento del dash
        this.DashCoolTime -= Time.deltaTime; // descontamo al tiempo de enfriamento un deltaTime
        if (this.DashCoolTime <= 0) {
            // si el tiempo de enfriamento llega a 0 ya podemos dashear
            this.CanDash = true;
        }
        else {
            // si no, no podemos
            this.CanDash = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && this.CanDash) {
            // Si apretamos la tecla "Espacio" y ya podemos dashear
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
                // Si al mismo tiempo de apretar la tecla espacio presionamos la tecla A (y solo la tecla A)
                // realizamos un dash hacia la izquierda
                this.DashVFX("Left"); // Llamamos al metodo que activa la animacion del dash y con su sentido izquierda

                // realizamos la accion del dash usando el LERP que es una interpolacion lineal desde un punto A a uno B en C steps
                this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, this.transform.position.x - this.DashDistance, this.DashStep),
                                                        this.transform.position.y,
                                                        this.transform.position.z);
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
                // Si al mismo tiempo de apretar la tecla espacio presionamos la tecla D (y solo la tecla D)
                this.DashVFX("Right"); // Llamamos al metodo que activa la animacion del dash y con su sentido derecha
                
                // misma accion pero hacia el otro lado 
                this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, this.transform.position.x + this.DashDistance, this.DashStep),
                                                        this.transform.position.y,
                                                        this.transform.position.z);
            }

            this.DashCoolTime = this.InitialDashCoolTime; // Si logramos hacer el dash el cool time vuelve a su valor inicial
        }
    }

    private void DashVFX(string dir) {
        Vector3 repos = new Vector3(1.5f, 0f, 0f); // para que la animacion sea mas limpia no se genera en la posicion del jugador sino desfasada
        if (dir == "Left") {
            // si el dash es hacia la izquierda la animacion es en ese sentido (y rotada)
            this.GetPool().Spawn("PlayerDash", this.transform.position - repos, Quaternion.identity * Quaternion.Euler(0f, 0f, 180f));
        }
        else {
            // si el dash es hacia la derecha la animacion se desafasa en ese sentido y sin rotacion
            this.GetPool().Spawn("PlayerDash", this.transform.position + repos, Quaternion.identity);
        }
    }

    private void MoveShield() {
        // Metodo que controla el movimiento del escudo
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            // si el jugador activa la rueda del mouse (-1 o 1)
            // pasamos al metodo de shield un valor de movimiento, se multiplica por 10 para que sea 1 o -1
            this.MyShield.ShieldControl(Convert.ToInt32(Input.GetAxis("Mouse ScrollWheel") * 10));
        }

        if (Input.GetMouseButtonDown(2)) {
            // si el jugador presiona la rueda del mouse se llama a un metodo de Shield que posiciona el escudo al frente
            this.MyShield.GetShieldOnFront(); 
        }

    }

    private void RestartShield() {
        // Metodo que controla el reinicio del shield cuando es destruido
        if (Input.GetKeyDown(KeyCode.Q)) {
            // si apretamos la tecla Q y el escudo no esta activo (enable)
            if (!this.MyShield.GetIsEnable()) {
                this.MyShield.RestartShield(0, new Vector3(1f, 1f, 1f), 1); // llamamos al metodo en shield de reinicio de escudo                
            }
        }
    }

    private void CheckMode() {
        // Metodo que controla el modo de la nave
        if (Input.GetKeyDown(KeyCode.LeftShift) && !this.InTransition) {
            // Si presionamos la tecla shift y no estamos en transicion
            // almacenamos la animacion de la transicion en una variable para poder asignarla como hija de la nave
            // en la jerarquia, de manera tal que si la nave se mueve la animacion lo hace con ella
            GameObject transition = this.GetPool().Spawn("TransitionAnimation", this.transform.position, this.transform.rotation);
            transition.transform.parent = this.transform;
            this.CanShoot = false; // quitamos la posibilidad de disparar mientras dure la transicion
            this.CanShootMissile = false; // quitamos la posibilidad de disparar misiles mientras dure la transicion
            // TODO: agregar costo de la transicion

            if (this.Mode == "Attack") {
                // si el modo actual al momento de apretar la tecla de cambio era de Ataque invocamos al metodo
                // que cambia el modo a defensivo
                Invoke("ChangeToDefenseMode", this.TransitionDelay);
            }
            else {
                // Por el contrario si el modo era defensivo invocamos al metodo para cambiar a modo de ataque
                Invoke("ChangeToAttackMode", this.TransitionDelay);
            }            
        }
    }

    private void ChangeToDefenseMode() {
        // Metodo que cambia el modo de la nave a defensiva
        this.MyAnimator.SetTrigger("DefenseState"); // Activa el sprite defensivo
        this.Mode = "Defense"; // Cambia el modo a defensivo
        this.ModifyValuesWithTransitions(this.Mode); // Metodo que modifica los valores de la nave
        this.ChangeCollidersAndEndTransition(true); // Metodo que cambia el colisionador y finaliza la transicion
    }

    private void ChangeToAttackMode() {
        // Metodo que cambia el modo de la nave a ofensiva
        this.MyAnimator.SetTrigger("AttackState"); // Activa el sprite ofensivo
        this.Mode = "Attack"; // Cambia el modo a ofensivo
        this.ModifyValuesWithTransitions(this.Mode);
        this.ChangeCollidersAndEndTransition(false);
    }

    private void ModifyValuesWithTransitions(string mode) {
        // Metodo que modifica los valores de la nave
        if(mode == "Attack") {
            // si el modo al que queremos pasar es ofensivo
            this.TimeBetweenBulletShoots /= (TransitionValueModifier * 2); // volvemos a valores iniciales
            this.SetVelocity(this.GetVelocity() / TransitionValueModifier);
            
        }
        else {
            // si el modo es defensivo
            this.TimeBetweenBulletShoots *= (TransitionValueModifier * 2); // se dispara mas lento
            this.RefillHealth(this.HitPoints * 2); // se recarga la vida al doble de la actual
            this.SetVelocity(this.GetVelocity() * TransitionValueModifier); // se aumenta la velocidad
        }

        // Esto evita el bug de cambiar la velocidad con un powerUp y que retorne en una velocidad distinta a la del modo dado
        this.SetOriginalVelocity(this.GetVelocity());
    }

    private void ChangeCollidersAndEndTransition(bool value) {     
        // Metodo que cambia los colisionadores y finaliza la transicion
        this.DefenseCollider.enabled = value;
        this.AtackCollider.enabled = !value;
        this.CanShootMissile = !value;
        this.CanShoot = true;
        this.InTransition = false;
    }

    
    

    public override void ControlOtherCollision(Collider2D collision) {
        // Metodo que controla colisiones de otro tipo
        if(collision.gameObject.tag == "Enemy") {
            this.HitPoints = 0;
            Die();            
        }
    }    

    
    public override void PlayImpactSFX() {
        // Metodo que controla el audio de los proyectiles
        this.GetPool().Spawn("ProyectileExplosion", this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(this.GetHurtSFX(), this.GetMyMainCamera().transform.position, 0.2f);
    }

    public override void Die() {
        // Metodo que controla la muerte del player
        this.ControlHealthBar(); // la barra de vida debe descargarse
        this.SetIsAlive(false); // el jugador se establece como "no vivo"
        this.PlayExplosion(); // Se muestra la explosion
        this.GetCameraShake().UltraShake(); // Llamamos a la clase SHake y hacemos un sacudon de camara         
        Destroy(gameObject); // Destruimos el objeto
    }

    private void PlayExplosion() {
        // Metodo de la explosion audio y visual del player
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(0.3f, 0.3f, 0f), Quaternion.identity);
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(-0.3f, 0.3f, 0f), Quaternion.identity);
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(0.3f, -0.3f, 0f), Quaternion.identity);
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(-0.3f, -0.3f, 0f), Quaternion.identity);

        AudioSource.PlayClipAtPoint(this.GetDeathSFX(), this.GetMyMainCamera().transform.position, 0.6f);
    }
    #endregion

    #region "Comportamientos PowerUps"
    private void UsePowerUp() {
        // Metodo que controla el comportamiento de los powerUps
        if (this.HasPowerUp) {
            // Si tenemos consumidos un powerUp y apretamos la tecla E
            if (Input.GetKeyDown(KeyCode.E)) {
                // Llamamos al tipo de powerUp consumido y usamos su metodo para que "haga su magia"
                this.PowerUpType.MakeYourMagic();
                this.HasPowerUp = false; // dejamos de tener un powerUp
            }
            
        }
    }
    
    #endregion

}
