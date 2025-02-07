const express = require('express');
const http = require('http');
const socketIo = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = require('socket.io')(server, {

});


let players = {}; // 플레이어 정보를 저장할 객체


io.on('connection', (socket) => {
    console.log('클라이언트 연결됨:', socket.id);

    // 새 플레이어가 연결될 때
    players[socket.id] = {
        id: socket.id,
        position: { 'x' : 0 , 'y': 0, 'z': 0 }
    };

    socket.emit("open");
    socket.on("test",(a) => {
        console.log(socket.id, " 클라이언트로부터 받음");
        socket.emit("newTest");
    })

    // 다른 클라이언트에게 새 플레이어 정보 전송
    socket.on("newPlayer", (a) => {
        console.log("두번째 패킷이 서버에 도착함");
    });

    socket.on('playerMovement', (movementData) => {
        // 플레이어의 움직임을 업데이트
        players[socket.id].position = movementData.position;
        // 다른 클라이언트에게 플레이어 움직임 정보 전송
        socket.to('room1').emit('playerMoved', players[socket.id]);
    });

    socket.on('disconnect', () => {
        console.log('클라이언트 연결 끊김:', socket.id);
        delete players[socket.id]; // 연결이 끊어진 플레이어 삭제
        socket.broadcast.emit('playerDisconnected', socket.id); // 다른 클라이언트에게 알림
    });
});

const PORT = 3333;
server.listen(PORT, () => {
    console.log(`서버가 http://local:${PORT} 에서 실행 중입니다.`);
});
