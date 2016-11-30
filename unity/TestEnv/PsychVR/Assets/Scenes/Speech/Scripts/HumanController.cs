using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HumanController : MonoBehaviour
{

    public GameObject humanPrefab;
    public GameObject[] humanModels;

    public RuntimeAnimatorController humanController;

    public Vector3 zeroVector = new Vector3(0, 0, 0);
    public Vector3 oneVector = new Vector3(1, 1, 1);
    public Quaternion zeroQuaternion = new Quaternion(0f, 0f, 0f, 0f);

    public Vector3[] animTransforms;

    public Vector3[] animRotations;

    public GameObject[] row1;
    public GameObject[] row2;
    public GameObject[] row3;
    public GameObject[] row4;

    //7.64f
    // Use this for initialization
    void Start()
    {
        List<int> nums = new List<int>();
        //if (seats == null)
        int firstAnimInt;
        string animations = PlayerPrefs.GetString("Animations");
		//animations = "[\"1\",\"2\",\"3\",\"4\",\"5\"]";
        Debug.Log(animations);
        if (animations == "" || animations == "[]")
        {
            firstAnimInt = Random.Range(1, 9);
        }
        else
        {
            firstAnimInt = 1;
            animations = animations.Substring(1);
            animations = animations.Substring(0, animations.Length - 1);
            animations = animations.Replace("\"", "");
            string[] list = animations.Split(',');
            foreach (string s in list)
            {
                nums.Add(System.Int16.Parse(s) + 1);
            }
            //Debug.Log(nums.Count);
            int idx = Random.Range(0, nums.Count);
            firstAnimInt = nums[idx];
            if (firstAnimInt == 5)
            {
                firstAnimInt = Random.Range(5, 9);
            }
        }
        
		//firstAnimInt = 

        //Debug.Log(animations);


        List<GameObject> unplacedHumans = new List<GameObject>();
        foreach (GameObject human in humanModels) unplacedHumans.Add(human);

        ArrayList rows = new ArrayList();
        rows.Add(row1);
        rows.Add(row2);
        rows.Add(row3);
        rows.Add(row4);

        int students = PlayerPrefs.GetInt("NumberStudents");
        //students = 13;
        int studentsRem = students;

        int rowCount = rows.Count;
        int seatCount = 0;
        foreach (GameObject[] row in rows) seatCount += row.Length;
        int seatsRem = seatCount;

        int firstPosition;

        //Place the front row middle student first
        if ((((GameObject[])rows[0]).Length) % 2 == 0)
        {
            firstPosition = Random.Range((((GameObject[])rows[0]).Length / 2) - 1, (((GameObject[])rows[0]).Length / 2) + 1);
        }
        else
            firstPosition = ((GameObject[])rows[0]).Length - 1;

        
        int humanSelector = Random.Range(0, unplacedHumans.Count);
        //float animSpeedMod = Random.Range(0.2f, 2f);

        //GameObject firstHuman = (GameObject)Instantiate(humanPrefab, zeroVector, zeroQuaternion);
        GameObject firstModel = (GameObject)Instantiate(unplacedHumans[humanSelector], zeroVector, zeroQuaternion);

        //firstModel.transform.parent = firstHuman.transform;

        firstModel.transform.parent = ((GameObject[])rows[0])[firstPosition].transform;
        firstModel.transform.localScale = oneVector;
        firstModel.transform.localEulerAngles = animRotations[firstAnimInt - 1];
        firstModel.transform.localPosition = animTransforms[firstAnimInt - 1];
        firstModel.GetComponent<Animator>().runtimeAnimatorController = humanController;
        firstModel.GetComponent<Animator>().applyRootMotion = false;
        firstModel.GetComponent<Animator>().SetInteger("position", firstAnimInt);
        //firstModel.GetComponent<Animator>().speed = firstModel.GetComponent<Animator>().speed * animSpeedMod;

        unplacedHumans.RemoveAt(humanSelector);
        studentsRem--;
        seatsRem--;

        //From back row to front row
        for (int i = rowCount - 1; i >= 0; i--)
        {
            //for controlled random likelihood which decreases each time

            //Length of this row
            int len = ((GameObject[])rows[i]).Length;

            //Which seats in this row have been considered
            bool[] consideredThis = new bool[len];
            int position;
            int j = 0;

            if (i == 0)
            {
                consideredThis[firstPosition] = true;
                j++;
            }

            //For as many seats are in this row
            for (; j < len;)
            {
                //Randomly choose the position
                position = Random.Range(0, len);

                //If we have not considered this seat yet
                if (!consideredThis[position])
                {
                    //Should a student be placed here (increasing likelihood by row against students to place)
                    if (studentsRem != 0 && prbStudent(seatCount, seatsRem, studentsRem, students, i + 1, rowCount))
                    {
                        int animInt = 1;
                        if (animations == "" || animations == "[]")
                        {
                            animInt = Random.Range(1, 9);
                        }
                        else
                        {
                            int idx = Random.Range(0, nums.Count);
                            animInt = nums[idx];
                            if (animInt == 5)
                            {
                                animInt = Random.Range(5, 9);
                            }
                        }
                        humanSelector = Random.Range(0, unplacedHumans.Count);
                        //animSpeedMod = Random.Range(0.1f, 0.8f);

                        //GameObject human = (GameObject)Instantiate(humanPrefab, zeroVector, zeroQuaternion);
                        GameObject model = (GameObject)Instantiate(unplacedHumans[humanSelector], zeroVector, zeroQuaternion);

                        //model.transform.parent = human.transform;

                        model.transform.parent = ((GameObject[])rows[i])[position].transform;
                        model.transform.localScale = oneVector;
                        model.transform.localEulerAngles = animRotations[animInt - 1];
                        model.transform.localPosition = animTransforms[animInt - 1];

                        model.GetComponent<Animator>().runtimeAnimatorController = humanController;
                        model.GetComponent<Animator>().applyRootMotion = false;
                        model.GetComponent<Animator>().SetInteger("position", animInt);
                        //model.GetComponent<Animator>().speed = model.GetComponent<Animator>().speed * animSpeedMod;

                        unplacedHumans.RemoveAt(humanSelector);
                        studentsRem--;
                    }
                    //We do not want to consider this seat again
                    consideredThis[position] = true;
                    seatsRem--;
                    //Try another seat
                    j++;
                }
            }
        }
    }

    public bool prbStudent(float seats, float seatsRem, float studentsRem, float students, float row, float rowCount)
    {
        if (seatsRem == studentsRem)
            return true;

        //Exponential curve from 0 to 1

        //Get the point along this curve (our probability) based on how many rows there are and which we are on, students being placed must impact!
        int p = (int)((((rowCount - row + 1) / (rowCount)) * ((rowCount + (rowCount / 2)) / (rowCount))) * (((students) / (seats + (seats * 0.1)))) * 10000f);
        //simulate an occurrance of p, if it happens true else false
        int sim = Random.Range(1, 10001);
        //Debug.Log(row + " " + p + " " + sim + " " + rowCount);

        if (sim <= p)
            return true;
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
