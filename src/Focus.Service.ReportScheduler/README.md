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

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/report/schedule/stats
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
{
    "totalSchedules"        : int,
    "executingSchedules"    : int,
    "outdatedSchedules"     : int,
    "futureSchedules"       : int
}
```

Returns basic statistics about `Report Schedule`s stored in service

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

From service directory (`src/Focus.Service.ReportScheduler/` in our case) run

```sh
docker build -t focus.scheduler .
```

Also this can be done via general production `docker-compose` file in `src` directory. Type following command

```sh
docker-compose build scheduler
```
