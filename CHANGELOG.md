# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [UNRELEASED]

## Changed

- Move runtime scripts to `Runtime` directory
- Only reference Unity Video module if it is enabled

## [1.3.1] - 2025-07-29

## Fixed

- Missing ResourceManager namespace error when not using Addressables

## [1.3.0] - 2024-10-31

## Changed

- Fall back to SceneManager if Addressables load fails (editor only).

## [1.2.0] - 2024-05-14

## Added

- Addressable scene load support (enable with `AELA_USE_ADDRESSABLES`)

## Changed

- Reference aseembly definitions by name instead of GUID

## [1.1.0] - 2024-05-13

## Added

- Filter log messages by severity

## Changed

- Make `ProgressiveOperationManager.Operation` disposable for ease of use
- Adopt MIT license

## [1.0.2] - 2023-12-19

## Fixed

- OnLoadProgress null reference exception

## [1.0.1] - 2023-03-20

## Fixed

- [LoadingScreen] Use UIFader coroutine-returning methods.

## [1.0.0] - 2023-01-16

### Added

- Basic scene transition system.
