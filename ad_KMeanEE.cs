using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mean_Project
{
    class ad_KMean_EE
    {
        double[] min_mean,max_mean,avg_mean;
        int[] local_assign;
        decimal averaged_distance;
        

        internal int[] get_good_current_mean(int no_of_clusters,int no_of_attributes,string input_file_name,int no_of_records)
        {
            int[] cluster_line_number = new int[no_of_clusters];
            // Changed from no.of cluster to 3 190712
            local_assign = new int[3];
            Console.WriteLine("Get the Good Current Mean {0}",no_of_clusters);
            
            min_mean = new double[no_of_attributes];
            max_mean = new double[no_of_attributes];
            avg_mean = new double[no_of_attributes];

            local_assign = get_min_max_average(input_file_name,no_of_records,no_of_attributes,no_of_clusters);
            if (no_of_clusters == 1)
            {
                cluster_line_number[0] = local_assign[1];
            }
            else if (no_of_clusters == 2)
            {
                cluster_line_number[0] = local_assign[0];
                cluster_line_number[1] = local_assign[2];
            }
            else if (no_of_clusters == 3)
            {
                cluster_line_number[0] = local_assign[0];
                cluster_line_number[1] = local_assign[1];
                cluster_line_number[2] = local_assign[2];
            }
            return cluster_line_number;
        }

        int[] get_min_max_average(string input_file_name, int no_of_records, int no_of_attributes, int no_of_clusters)
        {

            
                int min_line_number, max_line_number, avg_line_number, trace, new_min_line_number;
                double max_distance, distance_from_mean, distance_from_min_mean, distance_from_max_mean;
                double[] current_data = new double[no_of_attributes];
                decimal sum_of_distance;

                trace = 0;

                min_line_number = (no_of_records+1)/3;    // A


            here:                // for CD - Repetation after Changing the Value of "min_line_number" value with the Extreme Ended Point            
                trace++;             // for CD - Repetation

                string str;
                FileStream fs = new FileStream(input_file_name, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);

                for (int i = 1; i <= min_line_number; i++)
                {
                    str = sr.ReadLine();

                    string[] da;

                    da = str.Split(',');
                    for (int k = 0; k < no_of_attributes - 1; k++)
                    {
                        min_mean[k] = Convert.ToDouble(da[k]);
                    }

                }

                /*Console.WriteLine("Considered Min Mean");
                for (int k = 0; k < no_of_attributes - 1; k++)
                {
                    Console.Write("  {0}  ", min_mean[k]);
                }
                Console.WriteLine();*/
                sr.Close();
                // To Find the Maximum Distance Having Data Point

                max_line_number = 1;
                max_distance = 0;

                FileStream fs1 = new FileStream(input_file_name, FileMode.Open, FileAccess.Read);
                StreamReader sr1 = new StreamReader(fs1);
                sr1.BaseStream.Seek(0, SeekOrigin.Begin);

                for (int i = 1; i <= no_of_records; i++)
                {
                    distance_from_mean = 0;
                    str = sr1.ReadLine();

                    string[] da;

                    da = str.Split(',');
                    for (int k = 0; k < no_of_attributes - 1; k++)
                    {
                        current_data[k] = Convert.ToDouble(da[k]);
                        distance_from_mean = ((min_mean[k] - current_data[k]) * (min_mean[k] - current_data[k])) + distance_from_mean;

                    }
                    //Console.WriteLine("{0} Set {1}  {2}",trace, i,distance_from_mean);
                    if (distance_from_mean >= max_distance)
                    {
                        max_line_number = i;
                        max_distance = distance_from_mean;
                        for (int k = 0; k < no_of_attributes - 1; k++)
                        {
                            max_mean[k] = current_data[k];
                            //Console.WriteLine("Value {0}", max_mean[k]);
                            local_assign[2] = i;
                        }
                    }
                }
                //Console.WriteLine("{0} Set {1}",trace,local_assign[2]);
                sr1.Close();
                //Console.WriteLine("Max Distance Line Number is {0}", max_line_number);
                // This is the Mean Point for B
                // Now We need to Set the Mean Point for C by checking the Maximum Value Data Point (sum of distance from A and B)

                FileStream fs2 = new FileStream(input_file_name, FileMode.Open, FileAccess.Read);
                StreamReader sr2 = new StreamReader(fs2);
                sr2.BaseStream.Seek(0, SeekOrigin.Begin);
                max_distance = 0;
                for (int i = 1; i <= no_of_records; i++)
                {
                    distance_from_mean = 0;
                    distance_from_min_mean = 0;
                    distance_from_max_mean = 0;

                    str = sr2.ReadLine();

                    string[] da;

                    da = str.Split(',');
                    for (int k = 0; k < no_of_attributes - 1; k++)
                    {
                        current_data[k] = Convert.ToDouble(da[k]);
                        distance_from_min_mean = ((min_mean[k] - current_data[k]) * (min_mean[k] - current_data[k])) + distance_from_min_mean;
                        distance_from_max_mean = ((max_mean[k] - current_data[k]) * (max_mean[k] - current_data[k])) + distance_from_max_mean;
                        distance_from_mean = distance_from_min_mean + distance_from_max_mean;
                    }
                    if (distance_from_mean >= max_distance)
                    {
                        min_line_number = i;
                        max_distance = distance_from_mean;
                        for (int k = 0; k < no_of_attributes - 1; k++)
                        {
                            min_mean[k] = current_data[k];
                            //Console.WriteLine("Value {0}", max_mean[k]);
                        }
                        local_assign[0] = i;
                    }
                }
                sr2.Close();

                if (trace == 1)
                {
                    goto here;
                }

                /*Console.WriteLine("Value of Min Mean");
                for (int k = 0; k < no_of_attributes - 1; k++)
                {
                    Console.WriteLine("Value {0}", min_mean[k]);
                }

                Console.WriteLine("Value of Max Mean");
                for (int k = 0; k < no_of_attributes - 1; k++)
                {
                    Console.WriteLine("Value {0}", max_mean[k]);
                } 
                 With This We Found the Value for C and D.
                 * Now We need to Find the Average Point in the Given Data Set and Find the Appropriate Line Number
            
                 */

                FileStream fs3 = new FileStream(input_file_name, FileMode.Open, FileAccess.Read);
                StreamReader sr3 = new StreamReader(fs3);
                sr3.BaseStream.Seek(0, SeekOrigin.Begin);

                double minimum_distance = 0, maximum_distance = 0;

                sum_of_distance = 0;
                for (int i = 1; i <= no_of_records; i++)
                {
                    distance_from_mean = 0;
                    distance_from_min_mean = 0;
                    distance_from_max_mean = 0;


                    str = sr3.ReadLine();

                    string[] da;

                    da = str.Split(',');
                    for (int k = 0; k < no_of_attributes - 1; k++)
                    {
                        current_data[k] = Convert.ToDouble(da[k]);
                        distance_from_min_mean = ((min_mean[k] - current_data[k]) * (min_mean[k] - current_data[k])) + distance_from_min_mean;
                        distance_from_max_mean = ((max_mean[k] - current_data[k]) * (max_mean[k] - current_data[k])) + distance_from_max_mean;
                        distance_from_mean = distance_from_min_mean + distance_from_max_mean;
                    }
                    //Console.WriteLine("Value of C+D {0}  {1}", i,distance_from_mean);
                    if (i == 1)
                    {
                        minimum_distance = distance_from_mean;
                        maximum_distance = distance_from_mean;
                        local_assign[2] = 1;
                        local_assign[0] = 1;
                    }
                    if (distance_from_mean >= maximum_distance)
                    {
                        local_assign[2] = i;
                        maximum_distance = distance_from_mean;
                    }
                    if (distance_from_mean < minimum_distance)
                    {
                        local_assign[0] = i;
                        minimum_distance = distance_from_mean;
                    }
                    sum_of_distance = sum_of_distance + Convert.ToDecimal(distance_from_mean);
                }

                averaged_distance = sum_of_distance / no_of_records;
                Console.WriteLine();
                Console.WriteLine("Averaged Distance = {0}", averaged_distance);
                sr3.Close();



                FileStream fs4 = new FileStream(input_file_name, FileMode.Open, FileAccess.Read);
                StreamReader sr4 = new StreamReader(fs4);
                sr4.BaseStream.Seek(0, SeekOrigin.Begin);

                double current_difference = 0;

                for (int i = 1; i <= no_of_records; i++)
                {
                    distance_from_mean = 0;
                    distance_from_min_mean = 0;
                    distance_from_max_mean = 0;

                    str = sr4.ReadLine();

                    string[] da;

                    da = str.Split(',');
                    for (int k = 0; k < no_of_attributes - 1; k++)
                    {
                        current_data[k] = Convert.ToDouble(da[k]);
                        distance_from_min_mean = ((min_mean[k] - current_data[k]) * (min_mean[k] - current_data[k])) + distance_from_min_mean;
                        distance_from_max_mean = ((max_mean[k] - current_data[k]) * (max_mean[k] - current_data[k])) + distance_from_max_mean;
                        distance_from_mean = distance_from_min_mean + distance_from_max_mean;
                    }
                    if (i == 1)
                    {
                        current_difference = Math.Abs(distance_from_mean - Convert.ToDouble(averaged_distance));
                        local_assign[1] = 1;
                    }
                    if (Math.Abs(distance_from_mean - Convert.ToDouble(averaged_distance)) < current_difference)
                    {
                        current_difference = Math.Abs(distance_from_mean - Convert.ToDouble(averaged_distance));
                        local_assign[1] = i;
                    }



                }
                sr4.Close();


                Console.WriteLine("Record Number Min {0}", local_assign[0]);
                Console.WriteLine("Record Number Max {0}", local_assign[1]);
                Console.WriteLine("Record Number Max {0}", local_assign[2]);

               

            return local_assign;
        }

    }
}
