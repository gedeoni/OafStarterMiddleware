
{{- define "couchbase_password" -}}
{{ .Values.appSettings.Couchbase.Password }}
{{- end -}}

{{- define "couchbase_username" -}}
{{ .Values.appSettings.Couchbase.Username }}
{{- end -}}

{{- define "couchbase_connStr" -}}
{{ .Values.appSettings.ConnectionStrings.couchbase.data }}
{{- end -}}

{{- define "couchbase_bucket" -}}
{{ .Values.appSettings.Couchbase.BucketName }}
{{- end -}}

{{- define "couchbase_bucket_size" -}}
{{ .Values.couchbaseConfig.bucket_size }}
{{- end -}}

{{- define "couchbase_namespace" -}}
{{- $connStr := include "couchbase_connStr" . -}}
{{- $elements := regexSplit "[\\./:]+" $connStr -1 -}}
{{ slice $elements 2 | first }}
{{- end -}}

{{- define "couchbase_clusterName" -}}
{{- $connStr := include "couchbase_connStr" . -}}
{{- $elements := regexSplit "[\\./:]+" $connStr -1 -}}
{{ slice $elements 1 | first | trimSuffix "-srv" }}
{{- end -}}


{{- define "rabbitmq_config" -}}
{{  .Values.appSettings.RabbitMQ | toYaml }}
{{- end -}}

{{- define "rabbitmq_host" -}}
{{ .Values.appSettings.RabbitMQ.HostName }}
{{- end -}}

{{- define "rabbitmq_connStr" -}}
{{ .Values.appSettings.ConnectionStrings.rabbit.application }}
{{- end -}}

{{- define "rabbitmq_username" -}}
{{- $connStr := include "rabbitmq_connStr" . -}}
{{- $elements := regexSplit "[\\./:]+" $connStr -1 -}}
{{ slice $elements 1 | first }}
{{- end -}}

{{- define "rabbitmq_password" -}}
{{- $connStr := include "rabbitmq_connStr" . -}}
{{- $elements := regexSplit "[\\./:]+" $connStr -1 -}}
{{ slice $elements 2 3 | first }}
{{- end -}}

{{- define "rabbitmq_exchange" -}}
{{ get (include "rabbitmq_config" . | fromYaml) "Exchange" }}
{{- end -}}

{{- define "rabbitmq_key" -}}
{{ get (include "rabbitmq_config" . | fromYaml) "RoutingKeys" }}
{{- end -}}

{{- define "rabbitmq_port" -}}
{{ get (include "rabbitmq_config" . | fromYaml) "Port" }}
{{- end -}}
