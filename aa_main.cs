using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mean_Project
{
    public class aa_main
    {
        static void Main(string[] args)
        {
            ab_getdetails obj_ab = new ab_getdetails();
        here:
            obj_ab.get_data();

            obj_ab.set_current_mean(obj_ab.input_file_name);
            // To Get the Record Nos.for the Input File No. to select as Initial Mean

            obj_ab.initial_file_access(obj_ab.input_file_name, obj_ab.current_mean, obj_ab.output_file_name);
            // Generates Output File Considering the i) Input File and ii) Current Mean

            obj_ab.find_average_mean(obj_ab.output_file_name);
            // file_mean will get the cluster wise mean for the argument file name


            ac_put_on_loop obj_ac = new ac_put_on_loop();
            int no_of_clusters = obj_ab.n_no_clusters;
            int no_of_attributes = obj_ab.n_no_of_attributes;
            double[,] current_mean = new double[no_of_clusters, no_of_attributes];
            double[,] file_mean = new double[obj_ab.n_no_clusters, obj_ab.n_no_of_attributes];
            string input_file_name;
            string output_file_name;
            int trigger, repetation;
            int[] clus_count = new int[no_of_clusters];
            int[] min_avg_max = new int[3];

            current_mean = obj_ab.current_mean;
            file_mean = obj_ab.file_mean;
            clus_count = obj_ab.clus_count;


            //030912 - modi  obj_ac.get_required_accuracy();
            trigger = obj_ac.check_trigger(no_of_clusters, no_of_attributes, current_mean, file_mean);
            Console.WriteLine("Value of Trigger is {0}", trigger);

            int allowed_interation;
            Console.WriteLine("Please Enter the Maximum No. of Allowed Iteration for Generating the Output File");
            allowed_interation = Convert.ToInt32(Console.ReadLine());
            //New Items for SSE
            int sse;
            double sse_value_first=0, sse_value_next=0;
            ae_sse obj_ae=new ae_sse();
            {
                Console.WriteLine("Do you want to use SSE Values to Control the Iteration ?   1-Yes   0-No");
                sse=Convert.ToInt32(Console.ReadLine());
                if (sse == 1)
                {
                    sse_value_first = obj_ae.get_sse_value(obj_ab.output_file_name,obj_ab.n_no_clusters,obj_ab.n_no_of_attributes,obj_ab.n_no_of_records);                    
                }
            }

            for (int i = 2; (i < allowed_interation) && (trigger != 0) ; i++)
            {
                current_mean = obj_ab.current_mean;
                file_mean = obj_ab.file_mean;
                input_file_name = obj_ab.input_file_name;
                output_file_name = obj_ab.output_file_name;


                input_file_name = output_file_name;
                output_file_name = input_file_name + i;
                Console.WriteLine("Output File Name {0}", output_file_name);
                current_mean = file_mean;
                obj_ab.initial_file_access(input_file_name, current_mean, output_file_name);
                obj_ab.find_average_mean(output_file_name);
                trigger = obj_ac.check_trigger(no_of_clusters, no_of_attributes, current_mean, file_mean);
                repetation = obj_ac.check_converge(clus_count, obj_ab.clus_count, obj_ab.n_no_clusters);
                sse_value_next = obj_ae.get_sse_value(output_file_name, obj_ab.n_no_clusters, obj_ab.n_no_of_attributes,obj_ab.n_no_of_records);

                bool check;
                check = false;
                if (sse_value_first <= sse_value_next)
                {
                    check = true;
                    Console.WriteLine("Value of SSE in Previous File is Lesser than or Equal to the Current One Hence Stop Iterating");
                }

                if (repetation == 0|check==true)
                {
                    int opt;
                    if(check==true)
                        Console.WriteLine("Converged at {0}", i-1);
                    else
                        Console.WriteLine("Converged at {0}", i);
                    Console.WriteLine("Do you Want to Continue ?  0 No    1 Yes");
                    opt = Convert.ToInt32(Console.ReadLine());
                    if (opt == 1)
                        goto here;
                    else
                        break;
                }

                clus_count = obj_ab.clus_count;
                sse_value_first = sse_value_next;
            }

            Console.ReadLine();
        }
    }
}