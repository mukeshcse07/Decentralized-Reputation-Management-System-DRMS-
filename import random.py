import random
import hashlib
import time

from hedera import HederaClient


class DRMS:
    def __init__(self, client: HederaClient):
        self.client = client
        self.users = {}
        self.transactions = []
        self.feedback = {}

    def register_user(self, username, password):
        if username in self.users:
            raise ValueError("Username already exists")

        user_id = self._generate_user_id(username)
        self.users[username] = {
            "user_id": user_id,
            "password": password,
            "reputation": 0,
            "tokens": 0
        }

        return user_id

    def login(self, username, password):
        if username not in self.users:
            raise ValueError("Username does not exist")

        if self.users[username]["password"] != password:
            raise ValueError("Incorrect password")

        return self.users[username]

    def create_transaction(self, sender, recipient, amount):
        if sender not in self.users:
            raise ValueError("Sender does not exist")

        if recipient not in self.users:
            raise ValueError("Recipient does not exist")

        if amount <= 0:
            raise ValueError("Amount must be positive")

        transaction = {
            "sender": sender,
            "recipient": recipient,
            "amount": amount,
            "timestamp": time.time()
        }

        self.transactions.append(transaction)

        self.users[sender]["reputation"] += 1
        self.users[recipient]["reputation"] += 1

        self.client.submit_transaction(
            "transfer",
            account=sender,
            amount=amount,
            recipient=recipient
        )

    def give_feedback(self, user, feedback):
        if user not in self.users:
            raise ValueError("User does not exist")

        if feedback not in ["positive", "negative"]:
            raise ValueError("Invalid feedback")

        self.feedback[user] += 1 if feedback == "positive" else -1

        self.client.submit_transaction(
            "update_reputation",
            user_id=self.users[user]["user_id"],
            feedback=feedback
        )

    def get_reputation(self, username):
        if username not in self.users:
            raise ValueError("User does not exist")

        return self.users[username]["reputation"]

    def get_tokens(self, username):
        if username not in self.users:
            raise ValueError("User does not exist")

        return self.users[username]["tokens"]

    def _generate_user_id(self, username):
        return hashlib.sha256(username.encode()).hexdigest()


if __name__ == "__main__":
    client = HederaClient()
    drms = DRMS(client)

    user_id = drms.register_user("alice", "password")
    print("User ID:", user_id)

    drms.login("alice", "password")

    drms.create_transaction("alice", "bob", 10)

    drms.give_feedback("alice", "positive")

    print("Alice's reputation:", drms.get_reputation("alice"))
    print("Alice's tokens:", drms.get_tokens("alice"))
