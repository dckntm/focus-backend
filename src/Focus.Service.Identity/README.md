# Identity Service

Identity Service is responsible for managing `User`s & `Organization`s. ALso it is security entry point where our system logs `User`s in via producing personalized JWT Bearer Tokens.

## API

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

POST /api/identity/login
Content-Type: application/json
{
    "username" : string,
    "password" : string
}

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

string that contains JWT Bearer Token
```

Logs `User` in. As a return result it produces Bearer Json Web Token which should be placed to `Authorization` header: `Authorization: Bearer <token>`.

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

POST /api/identity/create HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json
{
    "name"                  : string,
    "surname"               : string,
    "patronymic"            : string,
    "organizationId"        : stringed ObjectId,
    "userRole"              : string      // possible values are "HOM", "COM", "HOA", "COA"
                                        // any other value will cause error result
}

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Pair: username & password of created user
```

Creates new `User`. Services creates logins & passwords on it's own and returns them to administrator when `User` is created. Login is usually Surname + Initials + Possibly some number. Password is generated & is not something readable (at least not on purpose).

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

POST /api/org/create
Authorization: Bearer <token>
Content-Type: application/json
{
    "id"            : string,     // doesn't matter (should be empty)
    "title"         : string,
    "isHead"        : bool,       // always false, if not error will be thrown
    "members"       : []string    // usernames of members (should be empty)
}

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Stringed id of new organization (which is initially MongoDB's ObjectId)
```

Creates new **`Child Organization`** as Head one is pre-created on service first run. If one will attempt to create Head `Organization` than service will throw an error.

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET /api/org/{id}
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
{
    "Id"            : string,     // stringed ObjectId
    "Title"         : string,
    "IsHead"        : bool,
    "Members"       : []string
}
```

Returns `Organization` model same as send to create new `Organization`.

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET /api/identity/{username}
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
{
    "name"              : string
    "surname"           : string
    "patronymic"        : string
    "username"          : string
    "password"          : string
    "organization"      : string    // stringed ObjectId
    "role"              : enum
    {
        0,                          // HeadOrganizationAdmin
        1,                          // ChildOrganizationAdmin
        2,                          // HeadOrganizationMember
        3                           // ChildOrganizationMember
    }
}
```

Returns `User` data including password (as this endpoint is accessible only by the `HOA`).

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET /api/identity/info
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
[
    {
        "name"              : string
        "surname"           : string
        "patronymic"        : string
        "username"          : string
        "password"          : string
        "organization"      : string    // stringed ObjectId
        "role"              : enum
        {
            0,                          // HeadOrganizationAdmin
            1,                          // ChildOrganizationAdmin
            2,                          // HeadOrganizationMember
            3                           // ChildOrganizationMember
        }
    }
]
```

Returns full information about all `User`s.

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET /api/org/info
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
[
    {
        "id"            : string
        "title"         : string
        "isHead"        : bool
    }
]
```

Returns short information about all `Organization`s registered in the service.

---

## Deploy

Application is deployed with Docker based on `focus_common` image which should be build first.

> If you already build latest version of `focus_common` image (which is required after each pull request as it's functionality is changing rapidly).

### Building common image

Build common image by running

```sh
docker build -t focus_common .
```

being in `src` folder of repository.

### Building Identity Service image

> Currently unavailable

From service directory (`src/Focus.Service.Identity/` in our case) run

```sh
docker build -t focus_identity .
```

### Local build

> This kind of build is for backend developers.

When building locally it's considered that we run this service individually & no other services were deployed on the same machine at the same time. To deploy locally we run

```sh
docker-compose -f docker-compose.local.yml up
```

from service folder (`src/Focus.Service.Identity/` in our case).

| Container Name   | Host             | Port        | Description                                                       |
| ---------------- | ---------------- | ----------- | ----------------------------------------------------------------- |
| `is_storage`     | `is_storage`     | 27017:27017 | MongoDB database where data is stored                             |
| `is_storage_gui` | `is_storage_gui` | 8081:8081   | MongoDB GUI interface that allows us to observe MongoDB databases |

In this case we run our service on our own via `dotnet run`.

### Development build

> Currently unavailable

> This kind of build is for frontend developers

When building for development we also consider that services are ran individually. Go to the service folder & run

```sh
docker-compose -f docker-compose.dev.yml up
```

| Container Name    | Host              | Port        | Description                                                       |
| ----------------- | ----------------- | ----------- | ----------------------------------------------------------------- |
| `is_storage`     | `is_storage`     | 27017:27017 | MongoDB database where data is stored                             |
| `is_storage_gui` | `is_storage_gui` | 8081:8081   | MongoDB GUI interface that allows us to observe MongoDB databases |
| `is`             | `localhost`       | 5000:5000   | Identity Service instance                                 |

### Production build

> TODO: add production build & description