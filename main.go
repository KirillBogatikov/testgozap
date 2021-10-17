package main

import (
	"go.uber.org/zap"
	"go.uber.org/zap/zapcore"
	"time"
)

func main() {
	cfg := zap.NewProductionConfig()
	cfg.EncoderConfig.TimeKey = "timestamp"
	cfg.EncoderConfig.EncodeTime = zapcore.RFC3339TimeEncoder

	log, _ := cfg.Build()
	defer log.Sync()

	sugar := log.Sugar()

	for {
		sugar.Infow("Hello world")
		time.Sleep(3 * time.Second)
	}
}
