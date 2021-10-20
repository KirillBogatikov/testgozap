package main

import (
	"github.com/KirillBogatikov/logger-go"
	"github.com/google/uuid"
	"math/rand"
	"time"
)

type S struct {
	Id   string `json:"id"`
	Code int    `json:"code"`
}

const dataFormat = `Lorem ipsum dolor sit amet, consectetur adipiscing elit, 
sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 
Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris 
nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in 
reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla 
pariatur. Excepteur sint occaecat cupidatat non proident, sunt in 
culpa qui officia deserunt mollit anim id est laborum. `

func main() {
	log := logger.NewZap(logger.NewFluentBit("http://192.168.5.4:5710", logger.DefaultTimeout).Lock(), logger.NewEncoderConfig(), "A")
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
			Id:   uuid.NewString(),
			Code: code,
		}

		sugar.Infow("Response received", "response", s)
		sugar.Infow("Multiline log", "data", dataFormat)
		time.Sleep(3 * time.Second)
	}
}
