package main

import (
	"bytes"
	"encoding/json"
	"github.com/KirillBogatikov/logger"
	"net/http"
	"time"
)

type Payload struct {
	Name  string `json:"name"`
	Phone string `json:"phone"`
}

func main() {
	log := logger.NewZap(logger.NewFluentBit("http://192.168.5.4:5710").Lock(), logger.NewEncoderConfig(), "A")
	defer func() { _ = log.Sync() }()
	sugar := log.Sugar()

	client := http.Client{
		Timeout: 15 * time.Second,
	}

	for {
		payload := &Payload{
			Name:  "Ivan Ivanov",
			Phone: "8-800-555-35-35",
		}

		data, err := json.Marshal(payload)
		if err != nil {
			sugar.Errorw("can't marshal payload", "error", err)
			continue
		}

		_, err = client.Post("https://ahaha.ahaha", "application/json", bytes.NewReader(data))
		if err != nil {
			sugar.Errorw("request failed", "error", err)
			continue
		}

		time.Sleep(5 * time.Second)
	}
}
