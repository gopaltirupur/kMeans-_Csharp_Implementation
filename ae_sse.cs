using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mean_Project
{
    class ae_sse
    {         

       public double get_sse_value(String file_name,int no_of_clusters,int no_of_attributes,int no_of_records)
        {
           double[,] file_mean;
            double sse_value=0;        
            int[] clus_count;

           // For Finding the Mean Value for the Given File
           {
               
            file_mean = new double[no_of_clusters, no_of_attributes];
            string str;
            double[] data = new double[no_of_attributes];
            clus_count = new int[no_of_clusters];
            //String path = (@"G:\JIT\PROJECTS\FILES\OUT\");
            //file_name = path + file_name;

            FileStream fs = new FileStream(file_name, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 1; i <= no_of_records; i++)
            {
                str = sr.ReadLine();
                string[] da;

                da = str.Split(',');

                for (int j = 0; j < no_of_attributes; j++)
                {
                    data[j] = Convert.ToDouble(da[j]);
                }
                // Last Element is the Cluster Number
                for (int j = 0; j < no_of_attributes - 1; j++)
                {
                    file_mean[(int)(data[no_of_attributes - 1]), j] = file_mean[(int)(data[no_of_attributes - 1]), j] + data[j];
                }
                clus_count[(int)(data[no_of_attributes - 1])] = (clus_count[(int)(data[no_of_attributes - 1])]) + 1;
                //Console.WriteLine(str);                
            }

            //Console.WriteLine("Mean for Selected File ( for  SSE VALUE FINDING ) {0} ", file_name);
            Console.Write("         ");
            for (int k = 0; k < no_of_attributes - 1; k++)
            {
                //Console.Write(" Attri{0} ", k);
            }
            //Console.WriteLine("No.Records");
            for (int j = 0; j < no_of_clusters; j++)
            {
                //Console.Write(" Clus{0} ", j);
                for (int k = 0; k < no_of_attributes - 1; k++)
                {
                    file_mean[j, k] = file_mean[j, k] / clus_count[j];
                    file_mean[j, k] = Math.Round(file_mean[j, k], 4);
                  //  Console.Write("  {0}  ", file_mean[j, k]);
                }
                //Console.Write(clus_count[j]);
                //Console.WriteLine();
            }
            sr.Close();
        }

           //For Finding the SSE Value
            {             
                    string str1;
                    double[] data = new double[no_of_attributes];                    
                    FileStream fs1 = new FileStream(file_name, FileMode.Open, FileAccess.Read);
                    StreamReader sr1 = new StreamReader(fs1);
                    sr1.BaseStream.Seek(0, SeekOrigin.Begin);
                    for (int i = 1; i <= no_of_records; i++)
                    {
                        str1 = sr1.ReadLine();
                        string[] da1;
                        da1 = str1.Split(',');
                        for (int j = 0; j < no_of_attributes; j++)
                        {
                            data[j] = Convert.ToDouble(da1[j]);
                        }
                        // Last Element is the Cluster Number
                        for (int j = 0; j < no_of_attributes-1; j++)
                        {
                            sse_value = sse_value + ((file_mean[(int)(data[no_of_attributes - 1]), j] - data[j]) * (file_mean[(int)(data[no_of_attributes - 1]), j] - data[j]));
                        }
                    }                    
                    sr1.Close();                
            }

            Console.WriteLine("SSE Value for the File Name :" + file_name + "   is  ");
            Console.WriteLine(sse_value);
            return sse_value;
        }
     }
}