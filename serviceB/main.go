package main

import (
	"github.com/KirillBogatikov/logger"
	"github.com/google/uuid"
	"math/rand"
	"time"
)

type S struct {
	Id   string `json:"id"`
	Code int    `json:"code"`
}

func main() {
	log := logger.NewZap(logger.NewFluentBit("http://192.168.5.4:24224").Lock(), logger.NewEncoderConfig(), "A")
	defer func() { _ = log.Sync() }()
	sugar := log.Sugar()

	for {
		r := rand.NormFloat64()
		var code int

		if r < 0.33 {
			code = 200
		} else if r < 0.66 {
			code = 201
		} else {
			code = 404
		}

		s := &S{
			Id: uuid.NewString(),
			Code: code,
		}

		sugar.Infow("Response received", "response", s)
		time.Sleep(3 * time.Second)
	}
}
