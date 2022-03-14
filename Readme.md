# Running the API and the unit tests

## Language requirements

make sure you have dotnet 6 sdk installed.
```
net6.0
```

**check your dotnet version**
```
dotnet --info
```
**expected result (>=6.0). Example:**
```
.NET SDK:
Version: 6.0.201
```

**Official link to download if needed (Windows, Mac, Linux or Docker)**
```
https://dotnet.microsoft.com/en-us/download
```

**clone the repository**
```js
git clone https://github.com/pauloharaujo/eVisitChallenge.git
```

**install the packages**
```js
dotnet restore
```

**running the API**
```js
dotnet run --project src/eVisit.Challenge.csproj
```

**sample endpoint calls**

```sh
# request_handled

curl -X 'GET' \
  'http://localhost:3000/request_handled?ip_address=127.0.0.1' \
  -H 'accept: */*'
```

```sh
# top100

curl -X 'GET' \
  'http://localhost:3000/top100' \
  -H 'accept: application/json'
```

```sh
# clear

curl -X 'GET' \
  'http://localhost:3000/clear' \
  -H 'accept: */*'  
```

# The challenge:

Imagine your team has developed a web service that receives requests from about 20 million unique IP addresses every day. You want to keep track of the IP addresses that are making the most requests to your service each day. Your job is to write a program that (1) tracks these IP addresses in memory (don’t use a database), and (2) returns the 100 most common IP addresses.

In the language of your choice, please implement these functions:

request_handled(ip_address)
- This function accepts a string containing an IP address like “145.87.2.109”. This function will be called by the web service every time it handles a request. The calling code is outside the scope of this project. Since it is being called very often, this function needs to have a fast runtime.

top100()
- This function should return the top 100 IP addresses by request count, with the highest traffic IP address first. This function also needs to be fast. Imagine it needs to provide a quick response (< 300ms) to display on a dashboard, even with 20 millions IP addresses. This is a very important requirement. Don’t forget to satisfy this requirement.

clear()
- Called at the start of each day to forget about all IP addresses and tallies.


# Please provide a short written description of your approach that explains:

**What would you do differently if you had more time?**
- At least I wanted to have implemented a better sorting algorithm than bubble sort.
- Following the TDD approach to implementing it would have been great and given me more confidence while refactoring the code.
- I would have added more test cases.
- Also, add IP validation and error handling.

**What is the runtime complexity of each function?**
- request_handled(ip_address): 
    - O(n).
    - Even though the dictionary is very fast to track all the IP addresses I am using the bubble sort to keep the top 100 most requested IPs. Because of it, the runtime was lowered down to O(n).
- top100()
    - O(1).
    - The top 100 IP addresses are already sorted in descending order, so I just need to return them.
- clear()
    - O(1).
    - Just creates a new instance for both objects used to track the IPs and top 100 most requested IPs.

**How does your code work?**
- I created an API using the new minimalistic API approach that was introduced on ASP.NET 6.
- The API exposes the 3 endpoints for the challenge requirements:
    - request_handled(ip_address)
    - top100()
    - clear()
- The unique IP Addresses are saved in memory using a Dictionary with the Key storing the IP Address for fast access and the Value stored the number of requests done by the corresponding IP.
- The Top 100 most requested IPs are saved on a list of IpAddressTracker which contains a property for storing the IP Address and the Number of Requests done by this IP Address.
- The API adds a dependency injection to the TrackIpAddressService and the API endpoints call each of its corresponding functions.
- The TrackIpAddressService implements the following functions:
    - ClearIpAddressTracking: 
        - Creates a new instance of the IP addresses dictionary and Top100IpAddresses List.
        - This is the function called by the clear() API endpoint.
    TrackNewIpAddress(string ipAddress):
        - Update the IP addresses dictionary by adding a new IP to it or updating an existing IP address entry.
        - Updates the Top100IpAddresses List by keeping the most requested sorted by descending order.
        - Updating the Top100IpAddresses always sorts the list using bubble sort which is O(n), but it only applies the sort to the Top 100 entries plus the new entry that is being added, so the sort would be applied to a maximum of 101 elements on the list. The latest element of the list is deleted when it has more than 100 items.

**What other approaches did you decide not to pursue?**
- I started using LINQ features to get the top 100 IPs sorted faster, but that is a C# specific feature and I wanted to implement it not relying on language-specific features.

**How would you test this?**
- I am using dependency injection when creating the API services, so I would mock the service interface to be able to make sure the requests are working as expected.
- Add more unit tests for the service so I could test it with millions of requests for example. I created some tests to make sure it is working, but arranging and asserting for millions of requests would take more time to implement.
- Also, I would create some load tests simulating multiple parallel requests to make sure the API has good performance when having high traffic.
