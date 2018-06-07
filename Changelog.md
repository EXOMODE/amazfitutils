# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).

## [1.0.2.12] - 2018-05-01
### Added
- Added support for air quality index value;
- Aanimated preview now shows discharging battery  instead of charging.

## [1.0.2.11] - 2018-05-01
### Changed
- Improved speed of all operations by tuning logging options

## [1.0.2.10] - 2018-05-01
### Added
- Skipped storing empty paramter lists. It was possible to brick watches with this :( (Thanks to Luca Venturini for the report)

## [1.0.2.9] - 2018-03-25
### Added
- Added sorting of image palette for correct repacking font images added in 0.1.1.15 firmware (RES 32) (#23)

## [1.0.2.8] - 2018-02-02
### Added
- Added support for unpacking/packing new format of .res-file from firmware 0.1.0.66 (#18)
### Changed
- Fixed drawing order of AM/PM element.

## [1.0.2.7] - 2018-01-18
### Added
- Added support for custom weather icons. New element `CustomIcon` was added to `Weather.Icon` element.
  It contains `X`, `Y`, `ImageIndex` and `ImagesCount` parameters.
  `X` and `Y` are the coordinates of the icon on the screen.
  `ImageIndex` and `ImagesCount` describe the images set used for weather icon.

## [1.0.2.6] - 2017-12-25
### Changed
- Fixed adding zero `DrawingOrder` when it's not present in config

## [1.0.2.5] - 2017-12-25
### Added
- Added support for digits drawing order in Time block used by new official watchface (Winter).

### Changed
- Renamed `Unknown3` property of `Weather.Icon` element to `CurrentAlt`.
- Renamed `Unknown3` and `Unknown4` properties of `Weather.Temperature.Today.Separate` element to `DayAlt` and `NightAlt`.

## [1.0.2.4] - 2017-12-14
### Changed
- Changed calculated block element position according to watches behavior. Text with width bigger than block width will be rendered with left alignment and text with height bigger than block height will be rendered with top alighnment.
- Fixed error "The image has (5,6,7...) bit/pixel". It was caused by not applying dithering to alpha-channel (#8).
- Dithering is now applied right after image load, so preview uses dithered instead of original image.

## [1.0.2.3] - 2017-12-10
### Changed
- Fixed packing .res files which was unpacked with latest version (using leading zeroes in image file names).

## [1.0.2.2] - 2017-12-09
### Added
- Used string representation of `"Alignment"` values. Possible text values are: `"TopLeft"`, `"TopCenter"`, `"TopRight"`, `"CenterLeft"`, `"Center"`, `"CenterRight"`, `"BottomLeft"`, `"BottomCenter"`, `"BottomRight"` (#7).
- Used JSON boolean values (`true` and `false`) for `"OnlyBorder"`, `"TwoDigitsMonth"`, `"TwoDigitsDay"` and `"AppendDegreesForBoth"`parameters.

### Changed
- Changed naming of extracted images to have 3 digits with leading zeroes (like 001.png instead of 1.png) (#9).
- Fixed displaying battery for 100% charged state for case when it was displayed by a set of images (#10).
- Fixed displaying of circle steps progress (#11).

## [1.0.2.1] - 2017-12-05
### Changed
- Fixed generating a preview for case when `Tens` or `Ones` are missing from `TwoDigits` block (#4)

## [1.0.2.0] - 2017-12-04
### Added
- Added generating animated previews.

### Changed
- Fixed drawing clock hands. They were skewed before.
- Improved speed of decoding/encoding images.

## [1.0.1.0] - 2017-12-02
### Added
- Generating preview on packing/unpacking watchface .

## [1.0.0.3] - 2017-11-29
### Added
- Added saving resources version on unpacking .res file.
- Added packing .res file (#1)

## [1.0.0.2] - 2017-11-28
### Added
- Added applying dithering for images on packing (#2)
- Added deleting incomplete bin if an error happened on packing.

## [1.0.0.1] - 2017-11-26
### Added
- Added ability to skip Off image for bluetooth and ability to add Off images for Lock, DND and Alarm elements.
- Added Net Framework 4.0 compatibility.

## [1.0.0.0] - 2017-11-26
### Added
- Implemented watchfaces unpacking and packing.
- Implemented .res file unpacking.

[Unreleased]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/HEAD..1.0.2.12
[1.0.2.12]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.12..1.0.2.11
[1.0.2.11]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.11..1.0.2.10
[1.0.2.10]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.10..1.0.2.9
[1.0.2.9]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.9..1.0.2.8
[1.0.2.8]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.8..1.0.2.7
[1.0.2.7]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.7..1.0.2.6
[1.0.2.6]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.6..1.0.2.5
[1.0.2.5]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.5..1.0.2.4
[1.0.2.4]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.4..1.0.2.3
[1.0.2.3]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.3..1.0.2.2
[1.0.2.2]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.2..1.0.2.1
[1.0.2.1]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.1..1.0.2.0
[1.0.2.0]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.2.0..1.0.1.0
[1.0.1.0]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.1.0..1.0.0.3
[1.0.0.3]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.0.3..1.0.0.2
[1.0.0.2]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.0.2..1.0.0.1
[1.0.0.1]: https://bitbucket.org/valeronm/amazfitbiptools/branches/compare/1.0.0.1..1.0.0.0