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

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/identity/stats
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
{
    "totalOrganizations"        : int,
    "totalUsers"                : int
}
```

Returns basic statistics about `User`s and `Organization`s stored in service

---

## Docker

Application is deployed with Docker based on `focus.common` image which should be build first.

### Building common image

Build common image by running

```sh
docker build -t focus.common .
```

being in `src` folder of repository

### Building Focus Report Template Service image

From service directory (`src/Focus.Service.Identity/` in our case) run

```sh
docker build -t focus.identity .
```

Also this can be done via general production `docker-compose` file in `src` directory. Type following command

```sh
docker-compose build identity
```