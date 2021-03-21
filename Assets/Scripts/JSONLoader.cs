using UnityEngine;

public class JSONLoader : MonoBehaviour
{
    public TextAsset jsonMag;

    public float[,,,] magnetisation;

    public bool debugging = true;

    public float[,,,] Magnet
    {
        get { return magnetisation; }
    }

    private void Awake()
    {
        magnetisation = ReadCSVMag();

        if (debugging) { Debug.Log($"{Magnet.GetLength(0)} {Magnet.GetLength(1)} {Magnet.GetLength(2)} {Magnet.GetLength(3)}"); }
        
    }
    

    public float[,,,] ReadCSVMag()
    {
        string data_string = jsonMag.ToString();
        data_string = data_string.Substring(0, data_string.Length - 5);
        data_string = data_string.Substring(4, data_string.Length - 4);

        string[] first_split = data_string.Split(new string[] { "]]],[[[" }, System.StringSplitOptions.None);

        int Nz = first_split[0].Split(new string[] { "]],[[" }, System.StringSplitOptions.None).Length;
        int Ny = first_split[0].Split(new string[] { "]],[[" }, System.StringSplitOptions.None)[0].Split(new string[] { "],[" }, System.StringSplitOptions.None).Length;
        int Nx = first_split[0].Split(new string[] { "]],[[" }, System.StringSplitOptions.None)[0].Split(new string[] { "],[" }, System.StringSplitOptions.None)[0].Split(',').Length;

        float[,,,] Mag = new float[Nx, Ny, Nz, first_split.Length];

        for (int i = 0; i < first_split.Length; i++)
        {
            string[] second_split = first_split[i].Split(new string[] { "]],[[" }, System.StringSplitOptions.None);

            for (int j = 0; j < second_split.Length; j++)
            {
                string[] third_split = second_split[j].Split(new string[] { "],[" }, System.StringSplitOptions.None);
                for (int k = 0; k < third_split.Length; k++)
                {
                    string[] fourth_split = third_split[k].Split(',');
                    for (int l = 0; l < fourth_split.Length; l++)
                    {
                        float value = float.Parse(fourth_split[l]);
                        Mag[l, k, j, i] = value;

                    }
                }
            }

        }

        if (debugging) { Debug.Log(Mag[0, 0, 0, 0]); }

        return Mag;
    }
}
