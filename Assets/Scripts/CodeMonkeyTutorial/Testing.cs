// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Trout.Utils;

// public class Testing : MonoBehaviour
// {
//     private Grid<TestGridObject > grid;
//     private void Start()
//     {
//         grid = new Grid<TestGridObject >(4, 2, 10f, Vector3.zero, (Grid<TestGridObject> g, int x, int y) => new TestGridObject(g,x,y));
//     }

//     private void Update(){
//         if(Input.GetMouseButtonDown(0)){
//             Vector3 position = Utils.GetMouseWorldPosition();
//             TestGridObject testGridObject = grid.GetGridObject(position);
//             Debug.Log(testGridObject);
//             if(testGridObject != null){
//                 testGridObject.AddValue(5);
//             }
//         }
//     }
// }

// public class TestGridObject  {

//     private const int MIN = 0;
//     private const int MAX = 100;

//     private Grid<TestGridObject> grid;
//     private int x;
//     private int y;
//     private int value;

//     public TestGridObject (Grid<TestGridObject> grid, int x, int y){
//         this.grid = grid;
//         this.x = x;
//         this.y = y;
//     }
//     public void AddValue(int addValue){
//         value += addValue;
//         value = Mathf.Clamp(value, MIN, MAX);
//         grid.TriggerGridObjectChanged(x, y);
//     }
//     public float GetValueNormalized(){
//         return (float)value / MAX;
//     }

//     public override string ToString(){
//         return value.ToString();
//     }
// }
