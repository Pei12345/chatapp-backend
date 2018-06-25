# Real-time chat app backend

ASP.NET Core 2.1, SignalR
Live demo running on Ubuntu 16.04 & Nginx @ https://chatapp.jussiporrassalmi.fi

### Setup

Setup database connection string

apsettings.json
```
  {
  "ConnectionStrings":{
    "DefaultConnection": "Server=localhost; Database=[Database name]; Uid=[Username]; Pwd=[password];"
  }
}
```