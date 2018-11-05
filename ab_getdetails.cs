using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mean_Project
{

    class ab_getdetails
    {
        internal static int no_of_attributes, no_of_clusters, no_of_records;
        internal int n_no_of_records, n_no_of_attributes, n_no_clusters;
        internal string in_file_name, in_path_name, input_file_name, ou_file_name, ou_path_name, output_file_name;
        internal Double[] data;
        internal double[,] file_mean;
        internal int[] clus_count;
        
        internal double[,] current_mean;

        internal void get_data()
        {
            Console.WriteLine("*** Ensure Last Column in the given dataset is an Identity of it ");
            Console.WriteLine("(LIKE CLUSTER INFO), not a Data ***");
            /* Console.WriteLine("Enter the No. of Attributes");
            no_of_attributes = Convert.ToInt32(Console.ReadLine());
            n_no_of_attributes = no_of_attributes;*/            
            Console.WriteLine("Enter the Input File Name");
            in_file_name = Console.ReadLine();
            
            string opt;
            Console.WriteLine("Do you Want to Specify the Path - YES OR NO");
            opt = Console.ReadLine();
            if (opt == "YES")
            {
                in_path_name = Console.ReadLine();
            }
            else
            {
                in_path_name = (@"G:\JIT\PROJECTS\FILES\IN\");
            }
            input_file_name = in_path_name + in_file_name;
            Console.WriteLine("Input File Name is{0}", input_file_name);

            Console.WriteLine("Enter the No. of Clusters");
            no_of_clusters = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Clusters Numbering is starting from 0 to (n-1)");            
            n_no_clusters = no_of_clusters;            

            {
                string str;
                string[] da;
                FileStream fs = new FileStream(input_file_name, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                str = sr.ReadLine();
                da = str.Split(',');
                no_of_attributes = Convert.ToInt32(da.Length);
                n_no_of_attributes = no_of_attributes;
                data = new Double[no_of_attributes];
                Console.WriteLine("No. of Attributes in the Given Data Set is  {0} ", n_no_of_attributes);
                sr.Close();
                data = new Double[no_of_attributes];
                file_mean = new double[no_of_clusters, no_of_attributes];
            }
            Console.WriteLine("Enter the Output File Name");
            ou_file_name = Console.ReadLine();

            Console.WriteLine("Do you Want to Specify the Path - YES OR NO");
            opt = Console.ReadLine();
            if (opt == "YES")
            {
                ou_path_name = Console.ReadLine();
            }
            else
            {
                ou_path_name = (@"G:\JIT\PROJECTS\FILES\OUT\");
            }

            output_file_name = ou_path_name + ou_file_name;
            Console.WriteLine("Output File Name {0}", output_file_name);
            no_of_records = get_no_of_records();
            Console.WriteLine("Total No. of Records in the File is {0}", no_of_records);
            n_no_of_records = no_of_records;
            Console.WriteLine("Do you Want to Fine Tune the Data ? \n * If Yes Press 1 *  * Else Press Other Integer *");
            int check;
            check = Convert.ToInt32(Console.ReadLine());
            if (check == 1)
            {
                fine_tune_input_file(input_file_name);
                input_file_name =in_path_name+"fi_"+in_file_name;
                Console.WriteLine("Finally Considered Input File Name After Fine Tuning   {0}", input_file_name);
            }
                        
        }

        void fine_tune_input_file(string input_file_name)
        {
            Console.WriteLine("Accessing Fine_Tune_Input_File");
        }

        int get_no_of_records()
        {
            string str;
            int count = 0;
            FileStream fs = new FileStream(input_file_name, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            str = sr.ReadLine();                      
            
            while (str != null)
            {
                str = sr.ReadLine();
                count++;
            }
            sr.Close();
            return count;
        }


        internal void find_average_mean(string file_name)
        {
            string str;
            double[] data = new double[no_of_attributes];
            clus_count= new int[no_of_clusters];
            FileStream fs = new FileStream(file_name, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 1; i <= n_no_of_records; i++)
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
            
            Console.WriteLine("Mean for Selected File {0} ",file_name);
            Console.Write("         ");
            for (int k = 0; k < no_of_attributes - 1; k++)
            {
                Console.Write(" Attri{0} ", k);
            }
            Console.WriteLine("No.Records");
            for (int j = 0; j < no_of_clusters; j++)
            {
                Console.Write(" Clus{0} ", j);
                for (int k = 0; k < no_of_attributes-1; k++)
                {
                    file_mean[j, k] = file_mean[j, k] / clus_count[j];
                    file_mean[j, k] = Math.Round(file_mean[j, k], 4);
                    Console.Write("  {0}  ",file_mean[j,k]);
                }
                Console.Write(clus_count[j]);
                Console.WriteLine();
            }            
            sr.Close();
        }

        public void set_current_mean(string input_file_name)
        {
            Console.WriteLine("Initial File Access ");
            int[] cluster_line_number = new int[no_of_clusters];
            current_mean = new double[no_of_clusters, no_of_attributes];
            int selection;

            selec:
            Console.WriteLine("Enter 1 If you Want to Give Random Mean as Current Mean");
            Console.WriteLine("Enter 2 If you Want to go for KMean EE (E-Extreme End)");            
            selection = Convert.ToInt32(Console.ReadLine());

            if (selection == 1)
            {

                for (int i = 0; i < no_of_clusters; i++)
                {
                here:
                    Console.WriteLine("As a Initial Consideration, Enter the Record No. for {0} th Cluster ", i);
                    cluster_line_number[i] = Convert.ToInt32(Console.ReadLine());
                    if ((cluster_line_number[i] > no_of_records) || (cluster_line_number[i] <= 0))
                    {
                        Console.WriteLine("Entered Record Number is Not in the Given File, Pl. Reenter");
                        goto here;
                    }
                }
            }

            else if(selection==2)
            {
                Console.WriteLine("K Mean EE");               
                ad_KMean_EE obj_ad_KMean_EE = new ad_KMean_EE();
                cluster_line_number = obj_ad_KMean_EE.get_good_current_mean(no_of_clusters,no_of_attributes,input_file_name,no_of_records);
            }

            else if ((selection != 1) && (selection != 2))
            {
                Console.WriteLine("Enter Only 1 or 2");
                goto selec;
            }

                for (int i = 0; i < no_of_clusters; i++)
                {
                    string str;
                    FileStream fs = new FileStream(input_file_name, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs);
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);

                    for (int j = 1; j <= cluster_line_number[i]; j++)
                    {
                        str = sr.ReadLine();

                        string[] da;


                        da = str.Split(',');
                        for (int k = 0; k < no_of_attributes - 1; k++)
                        {
                            current_mean[i, k] = Convert.ToDouble(da[k]);
                        }
                    }
                    sr.Close();
                }

                Console.WriteLine("Considered Current_Means Are (Based on Records Selected)");
                Console.Write("         ");
                for (int k = 0; k < no_of_attributes - 1; k++)
                {
                    Console.Write(" Attri{0} ", k);
                }
                Console.WriteLine("No.Records(PRM)");
                for (int i = 0; i < no_of_clusters; i++)
                {
                    Console.Write(" Clus{0} ", i);
                    for (int j = 0; j < no_of_attributes - 1; j++)
                    {
                        Console.Write("  {0}  ", current_mean[i, j]);
                    }
                    Console.WriteLine();
                }
        }
                    
        
        public void initial_file_access(string input_file_name,double[,] current_mean,string output_file_name)
        {          
            // Creating New File in the Name of Output File Name with I Level of Clustering
            {
                int count = 0;
                FileStream fs = new FileStream(input_file_name, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                double[] current_record = new double[no_of_attributes];

                FileStream fs_write = new FileStream(output_file_name, FileMode.Create, FileAccess.Write);
                StreamWriter sr_write = new StreamWriter(fs_write);
                
                for (int i = 1; i <= n_no_of_records; i++)
                {
                    string str,write_str;                    
                    str = sr.ReadLine();
                    int cluster_selection = 0;
                    if (str != null)
                    {
                        string[] da = null;

                        da = str.Split(',');
                        for (int k = 0; k < no_of_attributes - 1; k++)
                        {
                            current_record[k] = Convert.ToDouble(da[k]);
                        }

                        double[] compare_data_with_mean = new double[no_of_clusters];

                        for (int j = 0; j < n_no_clusters; j++)
                        {
                            for (int k = 0; k < n_no_of_attributes - 1; k++)
                            {
                                compare_data_with_mean[j] = compare_data_with_mean[j] + ((current_mean[j, k] - current_record[k]) * (current_mean[j, k] - current_record[k]));
                            }
                            compare_data_with_mean[j] = Math.Sqrt(compare_data_with_mean[j]);
                            //compare_data_with_mean[j] = (compare_data_with_mean[j]);
                            // With out square
                        }

                        double temp;
                        temp = compare_data_with_mean[0];
                        //Console.WriteLine(" {0} ", temp);
                        //Console.WriteLine(" {0} ", compare_data_with_mean[0]);

                        for (int q = 1; q < n_no_clusters; q++)
                        {
                            if (compare_data_with_mean[q]<temp)
                            {
                                //Console.WriteLine(compare_data_with_mean[i]);
                                //Console.WriteLine(temp);
                                temp = compare_data_with_mean[q];
                                cluster_selection = q;
                            }
                        }
                        //Console.WriteLine("Cluster Selected {0}", cluster_selection);
                    }
                    //Inserting the details in to the File
                    write_str=null;
                    
                    for (int p = 0; p < no_of_attributes-1; p++)
                    {
                        write_str = write_str + current_record[p] + ",";
                    }
                    write_str = write_str + cluster_selection;
                    sr_write.WriteLine(write_str);
                    //sr_write.WriteLine(cluster_selection);
                   // if (i <= n_no_of_records)
                    {
                        //sr_write.WriteLine(write_str);
                        count++;
                        //sr_write.Close();
                        //sr.Close();
                    }
                }
                Console.WriteLine("New File {0} Written, Total No. of Writes {1}", output_file_name,count);
                sr_write.Close();
                sr.Close();
            }
        }
    }
}