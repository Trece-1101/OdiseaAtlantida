using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject preFab;
        public int poolSize;
    }

    public static ObjectPool Instance;
    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDict;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        // creo un diccionario que como clave va a tener un string(tag)
        // y como valor una cola de tipo gameObject
        poolDict = new Dictionary<string, Queue<GameObject>>();

        // por cada elementto del tipo pool (con un tag, un sprite y un tamaño de pool)
        // que quiero crear creo una cola, itero sobre su tamaño y agrego la imagen a la cola
        // setActive = False hace que el objeto este invisible al momento de rellenar la cola
        foreach (var pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();

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
        if (!poolDict.ContainsKey(tag)) {
            return null;
        }

        // retiro del pool el primer elemento cargado (First In First Out)
        // lo activo y le doy una posicion y rotacion
        // por ultimo lo vuelvo a encolar para reutilizarlo
        GameObject spawnObject = poolDict[tag].Dequeue();
        spawnObject.SetActive(true);
        spawnObject.transform.position = position;
        spawnObject.transform.rotation = rotation;

        poolDict[tag].Enqueue(spawnObject);

        return spawnObject;
    }

}
