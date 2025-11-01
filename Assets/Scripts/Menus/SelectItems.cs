using UnityEngine;

public class SelectItems : MonoBehaviour
{
    public bool SelectBelt_1 = false;
    public bool SelectExtractor_2 = false;
    public bool SelectFishNet_3 = false;
    public bool SelectRiceField_4 = false;
    public bool SelectCutter_5 = false;
    public bool SelectStacker_6 = false;
    public bool SelectRoller_7 = false;

    public void selectOption(int option)
    {
        if (option == 1)
            SelectBelt_1 = true;
        else
            SelectBelt_1 = false;
        if (option == 2)
            SelectExtractor_2 = true;
        else
            SelectExtractor_2 = false;
        if (option == 3)
            SelectFishNet_3 = true;
        else
            SelectFishNet_3 = false;
        if (option == 4)
            SelectRiceField_4 = true;
        else
            SelectRiceField_4 = false;
        if (option == 5)
            SelectCutter_5 = true;
        else
            SelectCutter_5 = false;
        if (option == 6)
            SelectStacker_6 = true;
        else
            SelectStacker_6 = false;
        if (option == 7)
            SelectRoller_7 = true;
        else
            SelectRoller_7 = false;
    }
}
