package logger

import (
	"fmt"
	"go.uber.org/zap"
	"go.uber.org/zap/zapcore"
)

type Logger struct {
	log *zap.SugaredLogger
}

func NewEncoderConfig() zapcore.EncoderConfig {
	return zapcore.EncoderConfig{
		TimeKey:        "timestamp",
		LevelKey:       "level",
		NameKey:        "logger",
		CallerKey:      "caller",
		FunctionKey:    zapcore.OmitKey,
		MessageKey:     "message",
		StacktraceKey:  "stacktrace",
		LineEnding:     zapcore.DefaultLineEnding,
		EncodeLevel:    zapcore.CapitalLevelEncoder,
		EncodeTime:     zapcore.RFC3339TimeEncoder,
		EncodeDuration: zapcore.SecondsDurationEncoder,
		EncodeCaller: func(caller zapcore.EntryCaller, enc zapcore.PrimitiveArrayEncoder) {
			enc.AppendString(fmt.Sprintf("%s %s", caller.TrimmedPath(), caller.Function))
		},
	}
}

func NewZap(ws zapcore.WriteSyncer, config zapcore.EncoderConfig, name string) *zap.Logger {
	log := zap.New(zapcore.NewCore(
		zapcore.NewJSONEncoder(config),
		ws,
		zap.LevelEnablerFunc(func(level zapcore.Level) bool {
			return true
		}),
	), zap.AddCaller(), zap.Fields(zap.String("service", name)), zap.Development())

	return log
}
