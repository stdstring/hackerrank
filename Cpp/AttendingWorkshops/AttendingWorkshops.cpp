#include <algorithm>
#include <iostream>

// from https://en.wikipedia.org/wiki/Interval_scheduling : greedy solution
struct Workshop
{
public:
    int startTime, duration, endTime;
};

struct Available_Workshops
{
public:
    ~Available_Workshops()
    {
        delete[] workshops;
    }
    
    int n;
    Workshop *workshops;
};

Available_Workshops* initialize(int start_time[], int duration[], int n) 
{
    Available_Workshops *dest = new Available_Workshops();
    dest->n = n;
    dest->workshops = new Workshop[n];
    for (int index = 0; index < n; ++index)
    {
        dest->workshops[index].startTime = start_time[index];
        dest->workshops[index].duration = duration[index];
        dest->workshops[index].endTime = start_time[index] + duration[index];
    }
    auto comp = [](Workshop const &left, Workshop const &right)
    {
        return (left.endTime < right.endTime) || ((left.endTime == right.endTime) && (left.startTime < right.startTime));
    };
    std::sort(dest->workshops, dest->workshops + n, comp);
    return dest;
}

int CalculateMaxWorkshops(Available_Workshops* ptr)
{
    int count = 0;
    int currentEndTime = -1;
    for (int index = 0; index < ptr->n; ++index)
    {
        if (currentEndTime <= ptr->workshops[index].startTime)
        {
            currentEndTime = ptr->workshops[index].endTime;
            ++count;
        }
    }
    return count;
}

int main(int argc, char *argv[]) {
    int n; // number of workshops
    std::cin >> n;
    // create arrays of unknown size n
    int* start_time = new int[n];
    int* duration = new int[n];

    for (int i = 0; i < n; i++) {
        std::cin >> start_time[i];
    }
    for (int i = 0; i < n; i++) {
        std::cin >> duration[i];
    }

    Available_Workshops * ptr;
    ptr = initialize(start_time, duration, n);
    std::cout << CalculateMaxWorkshops(ptr) << std::endl;
    return 0;
}
