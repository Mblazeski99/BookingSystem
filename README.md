# BookingSystem

<h2>Authorization:</h2>

send post request to: https://localhost:7051/Auth/Authorize
with a 

```
Content-Type: application/json
```

and a body:

```
{
  "username": "someusername",
  "password": "somepassword"
}
```
to get a JWT Authorization token and add it as an Authorization Bearer Token Header for further requests.

<h2>Seach requests</h2>

<h3>Search HotelsOnly Example:</h3>

```
https://localhost:7051/Search?destination=SKP&from=2023-06-25&to=2023-07-10
```

<h3>Search HotelsAndFlights Example:</h3>
```
https://localhost:7051/Search?destination=SKP&from=2023-06-25&to=2023-07-10&departureAirport=CPH
```

<h3>Search LastMinuteHotels Example:</h3>
```
https://localhost:7051/Search?destination=SKP&from=2023-01-25&to=2023-02-10
```

<h2>Book request Example:</h2>
```
https://localhost:7051/Book?optionCode={code}
```
where 'code' is an optionCode from the result of any of the search requests.

<h3>Check Status Example:</h3>
```
https://localhost:7051/CheckStatus?bookingCode={code}
```
where 'code' is the booking code received upon completing the Book request.