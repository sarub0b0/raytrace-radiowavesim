# Radio wave propagation simulation
レイトレース法を使った３次元の電波伝搬シミュレーションです．


## 開発環境
Unity 5.5.2f1

## アプリの詳細
地形の生成は[Elevation Map](https://github.com/sarub0b0/elevation-map)で取得した標高データから生成しています．
MySQL周りのdllのバージョンが2.0しか使えなかったので，もしMySQLを使う場合はどっかから持って来る必要があります．

### 標高データから生成した地形
![earth](https://user-images.githubusercontent.com/23615151/35955203-51436c82-0cd1-11e8-86d0-06ec1ce6ed86.png)

### 手順
地形に適当にオブジェクトを置いて，送信点と受信点の位置を決定します．<br>
電波の送信出力，周波数，アンテナ利得，レイトレースの放射角分解能を設定してシミュレーションスタートです．

### Screenshot
赤い線が電波の通った経路です．<br>
![raytrace-ss](https://user-images.githubusercontent.com/23615151/35955483-f3076e3c-0cd2-11e8-8889-8564b0a34e7a.png)
