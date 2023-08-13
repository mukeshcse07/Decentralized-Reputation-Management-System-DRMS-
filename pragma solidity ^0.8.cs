pragma solidity ^0.8.0;

contract DRMS {

    struct User {
        string username;
        string password;
        uint256 reputation;
        uint256 tokens;
    }

    struct Transaction {
        string sender;
        string recipient;
        uint256 amount;
        uint256 timestamp;
    }

    mapping(string => User) public users;
    mapping(string => Transaction) public transactions;
    mapping(string => uint256) public feedback;

    function registerUser(string memory username, string memory password) public {
        if (users[username] != address(0)) {
            revert("Username already exists");
        }

        uint256 user_id = sha256(username);
        users[username] = User(username, password, 0, 0);

        emit UserRegistered(username, user_id);
    }

    function login(string memory username, string memory password) public {
        if (users[username] == address(0)) {
            revert("Username does not exist");
        }

        if (users[username].password != password) {
            revert("Incorrect password");
        }

        emit UserLoggedIn(username);
    }

    function createTransaction(string memory sender, string memory recipient, uint256 amount) public {
        if (users[sender] == address(0)) {
            revert("Sender does not exist");
        }

        if (users[recipient] == address(0)) {
            revert("Recipient does not exist");
        }

        if (amount <= 0) {
            revert("Amount must be positive");
        }

        Transaction memory transaction = Transaction(sender, recipient, amount, block.timestamp);
        transactions[transaction.id] = transaction;

        users[sender].reputation += 1;
        users[recipient].reputation += 1;

        emit TransactionCreated(transaction.id, transaction.sender, transaction.recipient, transaction.amount);
    }

    function giveFeedback(string memory user, string memory feedback) public {
        if (users[user] == address(0)) {
            revert("User does not exist");
        }

        if (feedback != "positive" && feedback != "negative") {
            revert("Invalid feedback");
        }

        feedback[user] += 1 if feedback == "positive" else -1;

        emit FeedbackGiven(user, feedback);
    }

    function getReputation(string memory username) public view returns (uint256) {
        if (users[username] == address(0)) {
            revert("User does not exist");
        }

        return users[username].reputation;
    }

    function getTokens(string memory username) public view returns (uint256) {
        if (users[username] == address(0)) {
            revert("User does not exist");
        }

        return users[username].tokens;
    }

    function _generateUserId(string memory username) internal pure returns (uint256) {
        return sha256(username);
    }

    event UserRegistered(string username, uint256 user_id);
    event UserLoggedIn(string username);
    event TransactionCreated(uint256 transaction_id, string sender, string recipient, uint256 amount);
    event FeedbackGiven(string user, string feedback);
}
