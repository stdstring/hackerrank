#include <iostream>
#include <deque>

struct MaxItem
{
public:
    MaxItem() : index(-1), value(0) {}
    MaxItem(int idx, int val) : index(idx), value(val) {}
    
    int index, value;
};

MaxItem findMax(int arr[], int start, int length)
{
    int maxIndex = start;
    int maxValue = arr[start];
    for (int index = 1; index < length; ++index)
    {
        if (maxValue <= arr[start + index])
        {
            maxIndex = start + index;
            maxValue = arr[start + index];
        }
    }
    return MaxItem(maxIndex, maxValue);
}

void printKMax(int arr[], int n, int k)
{
    //Write your code here.
    MaxItem current;
    for (int start = 0; start < n - k + 1; ++start)
    {
        if (start > current.index)
        {
            current = findMax(arr, start, k);
        }
        else
        {
            if (current.value <= arr[start + k - 1])
            {
                current.value = arr[start + k - 1];
                current.index = start + k - 1;
            }
        }
        std::cout << current.value << " ";
    }
    std::cout << std::endl;
}

int main(){
   int t;
   std::cin >> t;
   while(t>0) {
      int n,k;
       std::cin >> n >> k;
       int i;
       int *arr = new int[n];
       for(i=0;i<n;i++)
            std::cin >> arr[i];
       printKMax(arr, n, k);
       t--;
     }
     return 0;
}
