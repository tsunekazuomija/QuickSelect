# QuickSelect
Unityのヒエラルキー内のオブジェクトを素早く選択できるUnityエディター拡張

[English README is here](README.md)

## 導入方法
1. `QuickSelectWindow.cs`をUnityプロジェクトの`Editor`フォルダ内に配置します。
2. Unityのメニューバーから **Tools > QuickSelect** を選択し、QuickSelectウィンドウを開きます。

## 使い方
1. 登録したいオブジェクトを選択します（ヒエラルキー上でクリック）。
2. QuickSelectウィンドウで Register Selected Objects ボタンをクリックすると、選択したオブジェクト名が書かれたボタンがウィンドウ内に表示されます。
3. このボタンをクリックすると、対応するオブジェクトがヒエラルキー内で即座に選択されます。
4. シーンを切り替えても、元のシーンに戻ると登録したオブジェクトを選択可能な状態が保持されます。

## 注意
デフォルトでは、選択したオブジェクトはプロジェクトのルート下の `QuickSelect/SelectedGameObjects.json` に保存されます。
必要に応じて保存場所を変更できます。
また、保存先フォルダを.gitignoreなどに追加することで、バージョン管理対象から除外することができます。
