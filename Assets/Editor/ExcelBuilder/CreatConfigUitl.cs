﻿
using UnityEngine;
using System.IO;
using UnityEditor;

/**
 *   版权：博毅创为教育咨询有限公司  2018
 *   作者：Felix老师
 */
public class CreatConfigUitl {
    public static void CreatConfigFile(Object selectObj, string writePath)
    {
        string fileName = selectObj.name;
        string className = fileName;
        StreamWriter sw = new StreamWriter(Application.dataPath + writePath + className + ".cs");

        sw.WriteLine("using System.Collections;\n");
        sw.WriteLine("public class " + className);
        sw.WriteLine("{");

        string filePath = AssetDatabase.GetAssetPath(selectObj);
        CsvStreamReader csr = new CsvStreamReader(filePath);
        for (int colNum = 1; colNum < csr.ColCount + 1; colNum++)
        {
            string fieldName = csr[3, colNum];
            string fieldType = csr[2, colNum];
            sw.WriteLine("\t" + "public " + fieldType + " " + fieldName + ";" + "");
        }
        /*sw.WriteLine("\t" + "protected override string getFilePath ()");
        sw.WriteLine("\t" + "{");
        //		filePath=filePath.Replace("Assets/Resources/","");
        //		filePath=filePath.Substring(0,filePath.LastIndexOf('.'));
        sw.WriteLine("\t\t" + "return " + "\"" + fileName + "\";");
        sw.WriteLine("\t" + "}");
        */
        sw.WriteLine("}");

        sw.Flush();
        sw.Close();
        AssetDatabase.Refresh();        //这里是一个点
    }
}
