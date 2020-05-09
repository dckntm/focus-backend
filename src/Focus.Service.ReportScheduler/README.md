# Report Scheduler Service

Report Scheduler Service is responsible for acquiring & managing `Report Scheduler` which represent the way system emits new `Report` instances.

## API

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

POST: /api/report/schedule
Authorization: Bearer <token>
Content-Type: application/json
{
    "id"                        : string,   // empty ""
    "reportTemplate"            : string,   // not empty stringed MongoDB ObjectId
    "organizations":            []OrganizationAccess
    {
        "organization"          : string,   // not empty stringed MongoDB ObjectId
        "isDelegatedToCOA"      : bool,
        "assignees"             : []MemberAssignment
        {
            "user"              : string,   // not empty stringed MongoDB ObjectId
            "role"              : enum
            {
                Assignee,
                Viewer,
                Reviewer
            }
        }
    },
    "deadlinePeriod"            : string,   // "days.months.years"
    "emissionPeriod"            : string,   // "days.months.years"
    "emissionStart"             : string,   // "dd.MM.yyyy" considered local time
    "emissionEnd"               : string,   // "dd.MM.yyyy" considered local time
}

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

stringed ObjectId (MongoDB ID type)
```

Creates `Report Schedule` entity in database based on report

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/report/schedule/{id}
Authorization: Bearer <token>
where id : stringed ObjectId (MongoDB ID type)

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
{
    "id"                        : string,
    "reportTemplate"            : string,   // not empty stringed MongoDB ObjectId
    "organizations":            []OrganizationAccess
    {
        "organization"          : string,   // not empty stringed MongoDB ObjectId
        "isDelegatedToCOA"      : bool,
        "assignees"             : []MemberAssignment
        {
            "user"              : string,   // not empty stringed MongoDB ObjectId
            "role"              : enum
            {
                Assignee,
                Viewer,
                Reviewer
            }
        }
    },
    "deadlinePeriod"            : string,   // "days.months.years"
    "emissionPeriod"            : string,   // "days.months.years"
    "emissionStart"             : string,   // "dd.MM.yyyy" considered local time
    "emissionEnd"               : string,   // "dd.MM.yyyy" considered local time
}
```

Returns same dto model for `Report Schedule`

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/report/schedule/info
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
[
    {
        "id"                : string,       // stringed ObjectId
        "reportTemplate"    : string,       // stringed ObjectId
        "organizations"     : []string,     // list of stringed ObjectId
        "emissionPeriod"    : string,       // "dd.MM.yyyy-dd.MM.yyyy"
        "deadlinePeriod"    : string        // "days.months.years"
    }
]
```

Returns an array of report schedule dto info objects

---

## Deploy

Application is deployed with Docker based on `focus_common` image which should be build first.

### Building common image

Build common image by running

```sh
docker build -t focus.common .
```

being in `src` folder of repository

### Building Focus Report Scheduler Service image

From service directory (`src/Focus.Service.ReportScheduler/` in our case) run

```sh
docker build -t focus.scheduler .
```

### Local build

> This kind of build is for backend developers

When building locally it's considered that we run this service individually & no other services were deployed on the same machine at the same time. To deploy locally we run

```sh
docker-compose up
```

from service folder (`src/Focus.Service.ReportScheduler/` in our case).

| Container Name    | Host              | Port        | Description                                                       |
| ----------------- | ----------------- | ----------- | ----------------------------------------------------------------- |
| `rss_storage`     | `rss_storage`     | 27017:27017 | MongoDB database where data is stored                             |
| `rss_storage_gui` | `rss_storage_gui` | 8081:8081   | MongoDB GUI interface that allows us to observe MongoDB databases |

In this case we run our service on our own via `dotnet run`

### Development build

> This kind of build is for frontend developers

When building for development we also consider that services are ran individually. Go to the service folder & run

```sh
docker-compose up
```

| Container Name    | Host              | Port        | Description                                                       |
| ----------------- | ----------------- | ----------- | ----------------------------------------------------------------- |
| `rss_storage`     | `rss_storage`     | 27017:27017 | MongoDB database where data is stored                             |
| `rss_storage_gui` | `rss_storage_gui` | 8081:8081   | MongoDB GUI interface that allows us to observe MongoDB databases |
| `rss`             | `localhost`       | 5000:5000   | Report Scheduler Service instance                                 |

### Production build

> TODO: add production build & description
