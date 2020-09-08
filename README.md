# SeriesCapture

自动抓取剧集更新并添加到 aria2 下载任务

*目前仅能抓取 [追新番](http://zhuixinfan.com) 的剧集信息*

## 配置文件示例

```json
{
    "aria2Config": {
        "host": "http://localhost:6800/jsonrpc", // 你的 aria2 服务地址
        "token": "你的token"
    },
    "seriesData": [
        {
            //剧集 url
            "seriesUrl": "http://www.zhuixinfan.com/viewtvplay-1193.html",
            //下载目录
            "directory": "D:/Downloads/半泽直树2"
        },
        {
            "seriesUrl": "http://www.zhuixinfan.com/viewtvplay-1159.html",
            "directory": "D:/Downloads/美食侦探明智五郎"
        }
    ]
}
```

## 运行

双击运行默认会读取可执行文件所在目录的 config.json 配置，也可指定配置文件路径，示例：

```powershell
$ SeriesCapture "你的配置文件路径"
```

抓取任务每隔1小时运行一次
