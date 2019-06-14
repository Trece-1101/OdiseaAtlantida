using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool {
        // Esta clase Pool es la que se ve en el inspector del juego
        // Por eso sus atributos son public (para serializar la clase no se puede usar [SerializeField]
        public string tag; // Nombre del elemento prefabricado
        public GameObject preFab; // Objeto prefabricado
        public int poolSize; // Cantidad de objetos a instanciar
    }

    public static ObjectPool Instance;

    [SerializeField] private List<Pool> pools; // Una lista de objetos del tipo Pool

    private Dictionary<string, Queue<GameObject>> poolDict;
    private List<string> powersInPool = new List<string>(); // Una lista de los poderes dentro del pool

    private void Awake() {
        Instance = this;
    }

    public List<string> GetPrefabsPowerUps() {
        return this.powersInPool;
    }

    private void Start() {
        // creo un diccionario que como clave va a tener un string(tag)
        // y como valor una cola de tipo gameObject
        poolDict = new Dictionary<string, Queue<GameObject>>();

        // por cada elemento del tipo pool (con un tag, un sprite y un tamaño de pool)
        // que quiero crear creo una cola, itero sobre su tamaño y agrego la imagen a la cola
        // setActive = False hace que el objeto este invisible al momento de rellenar la cola
        foreach (var pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            if (pool.tag.Contains("PowerUp") && pool.poolSize > 1) {
                // Guardo en una lista todos los PowerUps (uso los tags)
                this.powersInPool.Add(pool.tag);
                //Debug.Log(pool.tag);
            }


            for (int i = 0; i < pool.poolSize; i++) {
                GameObject obj = Instantiate(pool.preFab) as GameObject;
                
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            // agrego a mi diccionario la clave(tag) y el valor(una cola)
            poolDict.Add(pool.tag, objectPool);
        }
    }

    public GameObject Spawn(string tag, Vector3 position, Quaternion rotation) {
        // Este es el metodo fundamental del juego, ante el llamado del metodo Spawn hay que pasar un string
        // que debe concidir con el tag, y una posicion de activacion y una rotacion

        if (!poolDict.ContainsKey(tag)) {
            // si no existe el tag devuelve un null
            return null;
        }
        // retiro del pool el primer elemento cargado (First In First Out)
        // lo activo y le doy una posicion y rotacion
        // por ultimo lo vuelvo a encolar para reutilizarlo
        GameObject spawnObject = poolDict[tag].Dequeue();
        if(spawnObject == null) {
            return null;
        }
        spawnObject.SetActive(true);
        spawnObject.transform.position = position;
        spawnObject.transform.rotation = rotation;

        poolDict[tag].Enqueue(spawnObject);

        // Retornar el objeto es fundamental cuando no solo queremos activarlo sino ademas hacer algo con el
        // Como en el caso de los drones transformarlos en hijo (jerarquia) de la nave del Player
        return spawnObject;       

    }

}
