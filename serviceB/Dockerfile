FROM golang:1.15-alpine

EXPOSE 80

WORKDIR /go/src/serviceB
COPY . .

RUN go install -mod vendor

ENTRYPOINT serviceB