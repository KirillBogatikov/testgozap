package main

import (
	"fmt"
	"go.uber.org/zap"
	"go.uber.org/zap/zapcore"
	"math/rand"
	"time"
)

type S struct {
	Name string `json:"name"`
	Code int    `json:"code"`
}

func main() {
	cfg := zap.NewProductionConfig()
	cfg.EncoderConfig.TimeKey = "timestamp"
	cfg.EncoderConfig.EncodeTime = zapcore.RFC3339TimeEncoder

	log, _ := cfg.Build()
	defer log.Sync()

	sugar := log.Sugar()

	for {
		s := &S{
			Name: fmt.Sprintf("Ivan-%d", rand.Int()),
			Code: rand.Intn(255),
		}

		if rand.Float64() > 0.75 {
			sugar.Errorw("Response failed", "response", s)
		} else {
			sugar.Infow("Response received", "response", s)
		}

		time.Sleep(3 * time.Second)
	}
}
