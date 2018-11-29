#include <iostream>
#include <vector>

struct MaxItem
{
public:
    MaxItem() : index(-1), value(0) {}
    MaxItem(int idx, int val) : index(idx), value(val) {}
    
    int index, value;
};

MaxItem findMax(std::vector<int> const &arr, int start, int length)
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

void printKMax(std::vector<int> const &arr, int n, int k)
{
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
   for (int testCase = 0; testCase < t; ++testCase)
   {
       int n, k;
       std::cin >> n >> k;
       std::vector<int> arr;
       arr.reserve(n);
       for(int index = 0; index < n; ++index)
            std::cin >> arr[index];
       printKMax(arr, n, k);
     }
     return 0;
}
