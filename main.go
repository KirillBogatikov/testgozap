package main

import (
	"go.uber.org/zap"
	"time"
)

func main() {
	log, _ := zap.NewProduction()
	defer log.Sync()

	sugar := log.Sugar()

	for {
		sugar.Infow("Hello world")
		time.Sleep(3 * time.Second)
	}
}