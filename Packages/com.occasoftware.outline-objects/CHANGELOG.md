# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.0.0] - 2023-07-06

This package is compatible with Unity 2022.3.0f1.

### Changed

- Switched to UPM-style package format.
- Restructured file hierarchy and renamed some files.
- Cleaned up add component menu.

### Added

- Added option to setup smooth normals with a component instead of editor utility.

## [2.1.1] - 2023-03-17

### Changed

- Changed the Outlines so that they now render to the Depth Only and Depth Normals passes.
- Changed the Outlines so that they now render correctly for VR Multi-Pass and SPI.

### Fixed

- Fixed an issue with the smoothing tool causing the baked mesh data to disappear when restarting the editor. The smoothing tool now creates a new mesh asset.
- Fixed an issue causing the outlines to fail to render when Depth Prepass is set to Enabled or Auto.

## [2.1.0] - 2023-03-16

### Added

- Added a new Smoothing tool that enables you to bake smooth normals to the UV3 channel of a mesh.
- Added a new Use Smoothed Normals option that works with the baked normals in the UV3 channel as an output of the tool.
- Added a demo showcasing the smooth normals in action.
- These new features together make it easier for you to use this asset meshes that have hard-edge shading (e.g., Unity default cube, low-poly art, etc.).

## [2.0.0] - 2023-02-27

### Changed

- Updated to 2021.3 LTS
- Improved Editor GUI
- Renamed some files
- Updated Readme

## [1.1.0] - N.D

### Changed

- Outlines now write to the depth buffer for compatibility with other assets like Altos and Buto

## [1.0.0] - N.D

- Initial release
