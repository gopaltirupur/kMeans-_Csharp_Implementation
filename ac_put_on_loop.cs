using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mean_Project
{
    class ac_put_on_loop
    {
        
        int sum_accuracy=0,trigger=0;
        double derived_accuracy_required=0;

        internal void get_required_accuracy()
        {
            Console.WriteLine("Enter the Required Accuracy Level in double format (for 2% enter 0.02)");
            derived_accuracy_required =Convert.ToDouble(Console.ReadLine());
            
            Console.WriteLine("Derived Accuracy {0}", derived_accuracy_required);
        }

        internal int check_trigger(int no_of_clusters,int no_of_attributes,double[,] current_mean,double[,] file_mean)
        {
            for (int i = 0; i < no_of_clusters; i++)
            {
                for (int j = 0; j < no_of_attributes-1; j++)
                {
                    if ((derived_accuracy_required*current_mean[i,j]) <= (Math.Abs(current_mean[i, j] - file_mean[i, j])))
                    {
                        Console.WriteLine("Triggered Value of i,j is {0}   {1}", i, j);
                        trigger++;
                    }
                }
            }
            Console.WriteLine("Status of Trigger is {0}", trigger);
            return trigger;
        }

        internal int check_converge(int[] clus_count1, int[] clus_count2,int no_of_clusters)
        {
            int repetation=0;
            for (int i = 0; i < no_of_clusters; i++)
            {
                if (!(clus_count1[i] == clus_count2[i]))
                {
                    repetation++;
                }
            }
            return repetation;
        }
    }
}
