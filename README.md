# Haikara

[English](#english)  
[日本語](#日本語)

## English

Haikara is an MVVM (Model-View-ViewModel) library designed for Unity's UI Toolkit.  
It enables data binding by writing simple view code.  
The goal is to eliminate the need to specify a DataSource in the UI Builder or write complex UI control logic.

The documentation is available [here](https://fireskyvvv.github.io/haikara-docs/en).

### Installation

#### Install via UPM (Git URL)

Please add the following URL by following
the [Unity documentation](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-ui-giturl.html).

```
https://github.com/fireskyvvv/Haikara.git#upm
```

You can also install it by directly editing Packages/manifest.json:

```
{
  "dependencies": {
    ...
    "com.katen.haikara": "https://github.com/fireskyvvv/Haikara.git#upm",
    ...
  }
}
```

#### Install via UnityPackage

Download the `.unitypackage` from the [releases](https://github.com/fireskyvvv/Haikara/releases) page and import it into
the Unity Editor.

### License
Haikara is released under the [MIT License](https://github.com/fireskyvvv/Haikara/blob/master/LICENSE.md).

## 日本語

Haikaraは、UnityのUIToolKit向けに設計されたMVVM（Model-View-ViewModel）ライブラリです。  
簡単なViewコードを記述することでデータバインディングを実現します。  
UI BuilderでDataSourceを指定したり、複雑なUI制御ロジックを書かなくて済むようになることを目指します。

ドキュメントは[**こちら**](https://fireskyvvv.github.io/haikara-docs/)です。

### 導入方法

#### UPMでのインストール (Git URL)

[Unityのドキュメント](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-ui-giturl.html)を参考に次のURLを追加してください

```
https://github.com/fireskyvvv/Haikara.git#upm
```

`Packages/manifest.json` を直接編集してインストールすることもできます。

```
{
  "dependencies": {
    ...
    "com.katen.haikara": "https://github.com/fireskyvvv/Haikara.git#upm",
    ...
  }
}
```

#### UnityPackageでのインストール

[リリース](https://github.com/fireskyvvv/Haikara/releases)ページから`.unitypackage`をダウンロードし、UnityEditor上で展開してください

### ライセンス
Haikaraは [MIT License](https://github.com/fireskyvvv/Haikara/blob/master/LICENSE.md)で公開しています。