# HotelSearch
JSON REST web service for hotel search
The service is made to be able to get, add, update, delete hotels from an existing list. In this case, the list is premade in the Controller, just to be able to test the application. 

API avaible:
* [GET] /api/Hotel
> Gets the list with all the Hotels without any condition       
* [GET] /api/Hotel/Count
> Gets the number of objects found in the existing Hotel list
* [GET] /api/Hotel/{hotelname}
> Search a the Hotel in the list by name. If not found return Bad Request with informative text
* [GET] /api/Hotel/{latitude}:{longitude}
> Sort the list of Hotels by price (low -> high) and distance (near -> far) and the return the sorted list
* [POST] /api/Hotel
> Add a new Hotel object in the existing list
* [PUT] /api/Hotel
> Update all the values changed on a specific Hotel. Update the complete Hotel object
* [DELETE] /api/Hotel
> Delete Hotel object using the Hotel object passed as parameter. Reeturns the list of hotels that remains after the delete
* [DELETE] /api/Hotel/ById
> Delete Hotel object using the Hotels Id passed as parameter. Reeturns the list of hotels that remains after the delete
