Trouble shoot

3/17/2021 Hashimoto

・トラッカーのBluetooth接続の問題
トラッカーをPCにUSBケーブルで接続しないと機能しない。

*Tracker isnot detected
Tracker should be connected by USB cable.


・コントローラー、トラッカーの対応が合ってない。適切じゃない
Steam VR_Tracked Object コンポーネントの Index を適切なものに設定しなければならない。（VR機器をセットアップするたびに、デバイス Index が変わる可能性あり）

*Controllers or Trackers arenot correspond
"Index" field in Steam VR_Tracked Object Component should be changed. (Index of device may change by setup VR)